// Ranged weapons that spawn projectiles, e.g. bows.
using System.Text;
using UnityEngine;
[CreateAssetMenu(menuName = "Item/Weapon(Ranged Projectile)", order = 999)]
public class RangedProjectileWeaponItem : RangedWeaponItem
{
    [Header("Projectile")]
    public Projectile projectile; // Arrows, rockets, etc.

    public override void Use(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt)
    {
        if (projectile != null)
        {
            if (hotbar != null)
            {
                Vector3 spawnPosition = hotbar.weaponMount.transform.position;
                Quaternion spawnRotation = hotbar.weaponMount.transform.rotation;

                GameObject go = Instantiate(projectile.gameObject, spawnPosition, spawnRotation);
                Projectile proj = go.GetComponent<Projectile>();
                proj.owner = hotbar.gameObject;
                proj.damage = damage;
                proj.direction = lookAt - spawnPosition;
                //NetworkServer.Spawn(go) // if we can get it to work
            }
            else Debug.LogWarning("hotbar was not found? How? Why?");
        }
        else Debug.LogWarning("projectile was not found");

        // base logic (decrease ammo)
        base.Use(hotbar, hotbarIndex, lookAt);
    }
}
