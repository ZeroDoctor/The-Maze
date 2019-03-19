using UnityEngine;
using UnityEngine.AI;

public class Magaani : Mob
{
    public NavMeshAgent agent;

    protected override string SpecialIdle()
    {
        action = "ROAR";

        specialEndTime = specialInterval + Time.time;
        return "IDLE";
    }

    private bool AttackBiteMethod()
    {
        return Random.value >= attackProbability;
    }

    protected override bool IsMoving()
    {
        return agent.pathPending ||
               agent.remainingDistance > agent.stoppingDistance ||
               agent.velocity != Vector3.zero;
    }
    private bool CanAttack(GameObject go)
    {
        return go.tag == "Player" && go.GetComponent<Health>().current > 0 && health.current > 0;
    }

    protected override bool EventAggro()
    {
        return target != null && target.GetComponent<Health>().current > 0;
    }

    protected override bool EventSpecialIdle()
    {
        if (specialEndTime <= Time.time) { action = "NOTHING"; }
        return Random.value <= specialProbability * Time.deltaTime;
    }

    protected override string ContinuousAttacking()
    {
        if (Vector3.Distance(transform.position, target.GetComponentInChildren<Collider>().ClosestPoint(transform.position)) <= interactRange * attackToMoveRangeRatio)
        {
            attackEndTime = Time.time + attackInterval; // Server time needed?
            ResetMovement();
            // RpcOnInteractStarted();
            return "INTERACTING";
        }

        return "MOVING";
    }

    protected override void ResetMovement()
    {
        agent.ResetPath();
        agent.velocity = Vector3.zero;
    }

    protected override string UpdateServer_Interacting()
    {
        if (target) LookAtY(target.transform.position);

        if (EventTargetDisappeared())
        {
            target = null;
            return "IDLE";
        }
        if (EventTargetDied())
        {
            // target died, stop attacking
            target = null;
            return "IDLE";
        }
        if (EventAttackFinished())
        {
            // finished attacking. apply the damage on the target
            combat.DealDamageAt(target, combat.damage, target.transform.position, -transform.forward, target.GetComponentInChildren<Collider>());

            // did the target die? then clear it so that the monster doesn't
            // run towards it if the target respawned
            if (target.GetComponent<Health>().current == 0)
                target = null;

            doneAttacking = true;
            action = "NOTHING";
            if (doneAttacking && AttackBiteMethod())
            {
                doneAttacking = false;
                action = "BITE";
            }
            else
            {
                action = "CLAW";
            }

            // go back to IDLE
            return "IDLE";
        }
        if (EventTargetTooFarToInteract())
        {
            Moving(runSpeed, interactRange * attackToMoveRangeRatio, target.GetComponentInChildren<Collider>().ClosestPoint(transform.position));
            return "MOVING";
        }
        if (EventTargetTooFarToFollow())
        {
            target = null;
            return "IDLE";
        }

        if ((action != "BITE" || action != "CLAW") && doneAttacking)
        {
            action = "CLAW";
        }

        return "INTERACTING";
    }

    protected override string UpdateServer_Dead()
    {
        // win condition trigger here
        return "DEAD";
    }

    protected override string Moving(float speed, float stoppingDistance, Vector3 goalDestination)
    {
        agent.speed = speed;
        agent.stoppingDistance = stoppingDistance;
        agent.destination = goalDestination;

        Debug.Log(goalDestination);

        return "MOVING";
    }

    public override void OnAggro(GameObject go)
    {
        if (go != null && CanAttack(go))
        {
            if (target == null)
            {
                target = go;
            }
            else if (target != go) // evaluate if we should target it
            {
                float oldDistance = Vector3.Distance(transform.position, target.transform.position);
                float newDistance = Vector3.Distance(transform.position, go.transform.position);
                if (newDistance < oldDistance * 0.8 &&
                    go.GetComponent<PlayerMovement>().soundCreated >
                    target.GetComponent<PlayerMovement>().soundCreated)
                {
                    target = go;
                }
            }
        }
    }

    public void OnReceivedDamage(GameObject attacker, int damageDealt)
    {
        OnAggro(attacker);
    }

}