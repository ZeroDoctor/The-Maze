// Ranged weapons that spawn projectiles, e.g. bows.
using System.Text;
using UnityEngine;
[CreateAssetMenu(menuName = "uSurvival Item/Weapon(Ranged Projectile)", order = 999)]
public class RangedProjectileWeaponItem : RangedWeaponItem
{
    [Header("Projectile")]
    public Projectile projectile; // Arrows, rockets, etc.

    public override void Use(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt)
    {
        if (projectile != null)
        {
            // spawn at muzzle location
            if (spawnlocation != null)
            {
                //Vector3 spawnPosition = details.muzzleLocation.position;
                Vector3 spawnPosition = spawnlocation.transform.position;
                Quaternion spawnRotation = spawnlocation.transform.rotation;

                GameObject go = Instantiate(projectile.gameObject, spawnPosition, spawnRotation);
                Projectile proj = go.GetComponent<Projectile>();
                proj.owner = hotbar.gameObject;
                proj.damage = damage;
                proj.direction = lookAt - spawnPosition;
                //NetworkServer.Spawn(go) // if we can get it to work
            }
            else Debug.LogWarning("weapon details or muzzle location not found for player: " + hotbar.name);
        }

        // base logic (decrease ammo and durability)
        base.Use(hotbar, hotbarIndex, lookAt);
    }
}
