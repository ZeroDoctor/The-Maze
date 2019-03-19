using UnityEngine;
using UnityEngine.AI;

public class MagaaniAnimation : MonoBehaviour
{

    public NavMeshAgent agent;
    public Magaani fsm;
    public Animator anim;

    void LateUpdate()
    {
        if (anim == null) return;
        anim.SetBool("IDLE", fsm.state == "IDLE");
        anim.SetFloat("Speed", agent.speed);
        anim.SetBool("MOVING", fsm.state == "MOVING" && agent.velocity != Vector3.zero);

        anim.SetBool("ATTACKING", fsm.state == "INTERACTING");
        anim.SetBool("CLAW", fsm.action == "CLAW");
        anim.SetBool("BITE", fsm.action == "BITE");

        anim.SetBool("DEAD", fsm.state == "DEAD");
        anim.SetBool("UNDER", fsm.action == "UNDER");
        if (fsm.action == "UNDER")
        {
            if (transform.position.y > 0)
            {
                anim.SetTrigger("DIG_DOWN");
            }
            else if (transform.position.y < 0)
            {
                anim.SetTrigger("DIG_UP");
            }
            else if (fsm.state == "INTERACTING")
            {
                anim.SetTrigger("ONE_HIT");
            }
        }

        anim.SetBool("ROAR", fsm.action == "ROAR");
    }


}