using UnityEngine;

public abstract class WeaponItem : UsableItem
{
    [Header("Weapon")]
    public float attackRange = 20;

    public int damage = 10;
    public string animationParameter;


    public override Usability CanUse(PlayerHotbar hotbar, int inventoryIndex, Vector3 lookAt)
    {
        return Usability.Never;
    }

    public override void Use(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt) { }
}