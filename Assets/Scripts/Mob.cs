using UnityEngine;


public abstract class Mob : FSM
{
    new public Collider collider;
    public Combat combat;

    [Header("Movement")]
    public float walkSpeed = 1;
    public float runSpeed = 5;
    protected float speed;

    [Range(0, 1)] public float moveProbability = 0.1f; // for random patrols
    [Range(0, 1)] public float specialProbability = 0.1f; // for roaring, dancing, etc.
    [Range(0, 1)] public float attackProbability = 0.1f; // for different types of attacks
    public float moveDistance = 10;

    public float followDistance = 0f;

    [Range(0.1f, 1)] public float attackToMoveRangeRatio = 0.5f;

    [Header("Interact")]
    public float interactRange = 0f;

    public float specialInterval = 1f;
    protected double specialEndTime;

    public float attackInterval = 0.5f;
    protected double attackEndTime;
    protected Vector3 startPosition;
    protected Vector3 destination;
    protected Vector3 velocity;
    protected float stoppingDistance;

    [HideInInspector]
    public string action = "NOTHING";
    protected bool doneAttacking = true;

    void Awake()
    {
        speed = walkSpeed;
        startPosition = transform.position;
        NavMeshAgentInit(startPosition);
    }

    protected virtual void NavMeshAgentInit(Vector3 startPosition) { }

    // events during a state ////////////////////////////////////////
    protected bool EventTargetDisappeared()
    {
        return target == null;
    }

    protected bool EventTargetDied()
    {
        return target != null && target.GetComponent<Health>().current == 0;
    }

    protected bool EventTargetTooFarToInteract()
    {
        return target != null && Utils.ClosestDistance(collider, target.GetComponentInChildren<Collider>()) > interactRange;
    }

    protected bool EventTargetTooFarToFollow()
    {
        return target != null && Vector3.Distance(startPosition, target.GetComponentInChildren<Collider>().ClosestPoint(transform.position)) > followDistance;
    }

    // I'm not sure if we need this since the walls might be thick enough
    protected bool EventTargetUnreachable()
    {
        return false;
    }

    // just minion and boss would use this
    protected bool EventAttackFinished()
    {
        // need network time so we know the rate of attack
        // Cilent side, mob versus player, combat is fine but
        // Server side time is definitely needed
        return Time.time >= attackEndTime;
    }

    protected abstract bool EventAggro();
    // return target != null && target.GetComponent<Health>().current > 0;

    protected bool EventMoveEnd()
    {
        return state == "MOVING" && !IsMoving();
    }

    protected abstract bool EventSpecialIdle(); // for special idle moves like roar or dance

    // ServerUpdates ////////////////////////////////////////////////////////////

    private string UpdateServer_Idle()
    {
        if (EventTargetDied())
        {
            target = null;
            return "IDLE";
        }
        if (EventTargetTooFarToFollow())
        {
            target = null;
            return Moving(walkSpeed, 0, startPosition);
        }
        if (EventTargetTooFarToInteract())
        {
            return Moving(runSpeed, interactRange * attackToMoveRangeRatio, target.GetComponentInChildren<Collider>().ClosestPoint(transform.position));
        }
        if (EventTargetUnreachable()) { } // not sure if this will ever happend
        if (EventAggro())
        {
            attackEndTime = Time.time + attackInterval; // Server time needed?
            // RpcOnInteractStarted(); if we want to play a sound
            return "INTERACTING";
        }
        if (EventSpecialIdle())
        {
            return SpecialIdle();
        }

        return "IDLE";
    }

    private string UpdateServer_Moving()
    {
        if (EventMoveEnd())
        {
            return "IDLE";
        }
        if (EventTargetDied())
        {
            target = null;
            Moving(walkSpeed, 0, startPosition);
            ResetMovement();
            return "IDLE";
        }
        if (EventTargetTooFarToFollow())
        {
            target = null;
            return Moving(walkSpeed, 0, startPosition);
        }
        if (EventTargetTooFarToInteract())
        {
            return Moving(runSpeed, interactRange * attackToMoveRangeRatio, target.GetComponentInChildren<Collider>().ClosestPoint(transform.position));
        }
        if (EventTargetUnreachable()) { } // not sure if this will ever happend
        if (EventAggro())
        {
            return ContinuousAttacking();
        }

        return "MOVING";
    }

    // merchant mob would have this function empty
    protected abstract string ContinuousAttacking();

    // some mobs sell to the player while other mobs attack the player
    protected abstract string UpdateServer_Interacting();

    // merchant mob can't die so this function would be empty for him
    // also the game ends if the main boss dies
    protected abstract string UpdateServer_Dead();

    protected override void UpdateClient()
    {
        if (state == "INTERACTING")
        {
            if (target) LookAtY(target.transform.position); // so, mob always face the player when selling or attacking
        }
    }

    protected override string UpdateServer()
    {
        if (health.current <= 0)
        {
            target = null;
            return UpdateServer_Dead();
        }
        else
        {
            if (state == "IDLE") return UpdateServer_Idle();
            if (state == "MOVING") return UpdateServer_Moving();
            if (state == "INTERACTING") return UpdateServer_Interacting();
        }

        // if somehow we are in a different state not mention above then:
        return "IDLE";
    }

    public void OnDeath()
    {
        state = "DEAD";
        ResetMovement();
        target = null;
    }

    public void LookAtY(Vector3 position)
    {
        transform.LookAt(new Vector3(position.x, transform.position.y, position.z));
    }

    protected abstract bool IsMoving();

    protected abstract string SpecialIdle();

    // mag-aani will override this
    protected virtual void ResetMovement() { }

    // merchant would stop and play the bells instead of moving towards the player
    protected abstract string Moving(float speed, float stoppingDistance, Vector3 destination);

}