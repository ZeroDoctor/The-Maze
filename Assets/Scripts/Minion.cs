using UnityEngine;


public class Minion : Mob
{

    private bool CanAttack(GameObject go)
    {
        return go.tag == "Player" && go.GetComponent<Health>().current > 0 && health.current > 0;
    }

    protected override bool EventAggro()
    {
        return target != null && target.GetComponent<Health>().current > 0;
    }

    protected override bool ContinuousAttacking()
    {
        if (Vector3.Distance(transform.position, target.GetComponentInChildren<Collider>().ClosestPoint(transform.position)) <= interactRange * attackToMoveRangeRatio)
        {
            attackEndTime = Time.time + attackInterval; // Server time needed?
            ResetMovement();
            // RpcOnInteractStarted();
            return true;
        }

        return false;
    }

    protected override string UpdateServer_Interacting()
    {
        // always look at the target. Eye contact is important
        if (target) LookAtY(target.transform.position);

        if (EventTargetDisappeared())
        {
            target = null;
            return "IDLE";
        }
        if (EventTargetDied())
        {
            target = null;
            return "IDLE";
        }
        if (EventAttackFinished())
        {
            // !**Combat stuff
            if (target.GetComponent<Health>().current == 0)
            {
                target = null;
            }

            return "IDLE"; // to prevent minions from attacking at ridiculous speeds
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

        return "INTERACTING";
    }

    protected override string UpdateServer_Dead()
    {
        return "DEAD";
    }

    protected override string Moving(float speed, float stoppingDistance, Vector3 destination)
    {
        if (target != null)
        {
            LookAtY(target.transform.position);

            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(destination.x - stoppingDistance, destination.y, destination.z - stoppingDistance),
                speed * Time.deltaTime);
        }

        return "MOVING";
    }

    // Should be RPC call
    public void OnReceivedDamage(GameObject attacker, int damageDealt)
    {
        OnAggro(attacker);
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
                if (newDistance < oldDistance * 0.8)
                {
                    target = go;
                }
            }
        }
    }

}