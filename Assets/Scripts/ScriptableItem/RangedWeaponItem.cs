// Guns, bows, etc.
using System.Text;
using UnityEngine;

public abstract class RangedWeaponItem : WeaponItem
{
    public AmmoItem requiredAmmo;
    public float zoom = 20;
    public GameObject spawnlocation;

    [Header("Recoil")]
    [Range(0, 30)] public float recoilHorizontal;
    [Range(0, 30)] public float recoilVertical;

    // usage
    public override Usability CanUse(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt)
    {
        // reloading or not enough time since last attack?
        if (Time.time < hotbar.usageEndTime)
            return Usability.Cooldown;

        // not enough ammo?
        if (requiredAmmo != null && hotbar.slots[hotbarIndex].item.ammo == 0)
            return Usability.Empty;

        // otherwise we can use it
        return Usability.Usable;
    }

    // helper functions ////////////////////////////////////////////////////////

    protected bool RaycastToLookAt(GameObject player, Vector3 lookAt, out RaycastHit hit)
    {
        Transform head = player.GetComponent<Animator>().GetBoneTransform(HumanBodyBones.Head);

        // ignore self just to be sure
        Vector3 direction = lookAt - head.position;

        if (Utils.RaycastWithout(head.position, direction, out hit, attackRange, player))
        {
            return true;
        }

        hit = new RaycastHit();
        return false;
    }

    public override void Use(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt)
    {
        ItemSlot slot = hotbar.slots[hotbarIndex];

        // decrease ammo (if any is required)
        if (requiredAmmo != null)
        {
            --slot.item.ammo;
            hotbar.slots[hotbarIndex] = slot;
        }
    }

    public override void OnUsed(PlayerHotbar hotbar, Vector3 lookAt)
    {
        // recoil, decals, and/or audio goes here
    }
}
