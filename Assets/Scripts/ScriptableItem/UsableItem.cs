using UnityEngine;


public enum Usability : byte { Usable, Cooldown, Empty, Never }

public abstract class UsableItem : ScriptableItem
{

    [Header("Usage")]

    public bool keepUsingWhileButtonDown; // used for weapons, etc.
    public float cooldown; // used for bows fire rate, magic spells, and potion usage
    public bool useDirectly; // used for potions

    public abstract Usability CanUse(PlayerHotbar hotbar, int inventoryIndex, Vector3 lookAt);

    public abstract void Use(PlayerHotbar hotbar, int hotbarIndex, Vector3 lookAt);

    // Used for sounds
    public virtual void OnUsed(PlayerHotbar hotbar, Vector3 lookAt) { }
}