// Projectile skill effects like arrows, flaming fire balls, etc. that deal
// damage on the target.
using UnityEngine;

// needs a rigidbody to detect OnTriggerEnter etc.
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour
{
    public Rigidbody rigidBody;
    new public Collider collider;

    public float speed = 1;
    public float destroyAfter = 10; // don't let it fly forever. destroy if it hits nothing.
    [HideInInspector] public GameObject owner; // probably need to sync this
    [HideInInspector] public Vector3 direction; // probably need to sync this
    [HideInInspector] public int damage = 1;

    void Start()
    {
        // ignore collisions with all the caster's colliders, so we don't
        // collide with the hand, etc.
        foreach (Collider co in owner.GetComponentsInChildren<Collider>())
            Physics.IgnoreCollision(collider, co);


        Invoke("DestroySelf", destroyAfter);
    }

    void FixedUpdate()
    {
        // move rigidbody and look at direction
        rigidBody.MovePosition(Vector3.MoveTowards(transform.position, transform.position + direction, speed * Time.fixedDeltaTime));
        transform.LookAt(transform.position + direction);
    }

    void OnTriggerEnter(Collider co)
    {
        // only deal damage on server

        // hit something with a combat component?
        Health health = co.GetComponentInParent<Health>();
        Combat combat = co.GetComponentInParent<Combat>();
        if (health != null && combat != null && health.current > 0)
        {
            Combat casterCombat = owner.GetComponent<Combat>();
            casterCombat.DealDamageAt(health.gameObject,
                                        casterCombat.damage + damage, // amount
                                        transform.position, // hitPoint
                                        -direction, // hitNormal
                                        co); // hitCollider
        }

        // destroy projectile in any case. doesn't matter if we collided with a
        // monster, a house, terrain, etc.
        Destroy(gameObject);
    }

    void DestroySelf()
    {
        Destroy(gameObject);
    }
}