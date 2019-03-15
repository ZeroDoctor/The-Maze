using UnityEngine;

public class AggroArea : MonoBehaviour
{

    public Mob owner;

    void OnTiggerEnter(Collider co)
    {
        Health health = co.GetComponentInParent<Health>();
        if (health) owner.OnAggro(health.gameObject);
    }

    void OnTriggerStay(Collider co)
    {
        Health health = co.GetComponentInParent<Health>();
        if (health) owner.OnAggro(health.gameObject);
    }

}