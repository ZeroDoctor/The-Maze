using System.Text;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/MeleeWeaponItem", order = 999)]
public class MeleeWeaponItem : WeaponItem
{
    public float sphereCastRadius = 0.5f;
    public override Usability CanUse(PlayerHotbar hotbar, int inventoryIndex, Vector3 lookAt)
    {
        return Time.time >= hotbar.usageEndTime &&
            hotbar.CoolDownTimeRemaining() == 0
            ? Usability.Usable : Usability.Cooldown;
    }

    private Health SphereCastToLookAt(GameObject go, Collider collider, Vector3 lookAt, out RaycastHit hit)
    {
        Vector3 origin = collider.bounds.center;
        Vector3 behindOrigin = origin - go.transform.forward * sphereCastRadius; // because spherecast ignores collides that it starts in
        Vector3 direction = (lookAt - origin).normalized;
        if (Utils.SphereCastWithout(behindOrigin, sphereCastRadius, direction, out hit, attackRange + sphereCastRadius, go))
        {
            return hit.transform.GetComponent<Health>();
        }
        return null;
    }

    public override void Use(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt)
    {
        Combat combat = hotbar.GetComponent<Combat>();
        RaycastHit hit;
        Health enemyHealth = SphereCastToLookAt(hotbar.gameObject, hotbar.GetComponentInChildren<CapsuleCollider>(), lookAt, out hit);
        if (enemyHealth != null)
        {
            combat.DealDamageAt(enemyHealth.gameObject, combat.damage + damage, hit.point, hit.normal, hit.collider);
        }
    }

}