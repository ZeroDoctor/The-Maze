using UnityEngine;
using System;
using System.Linq;

public class PlayerHotbar : MonoBehaviour, ICombatBonus
{

    public Animator animator;

    public Health health;

    public Combat combat;
    public PlayerMovement movement;
    public PlayerLook look;
    public int size = 4;
    public ListItemSlot slots = new ListItemSlot();
    public ScriptableItem[] defaultItems;

    // number keys on top row
    public KeyCode[] keys = { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4 };

    public int selection = 0;

    [HideInInspector]
    public double usageEndTime;

    private double cooldownTimeEnd;

    public MeleeWeaponItem hands;

    public float CoolDownTimeRemaining()
    {
        return Time.time >= cooldownTimeEnd ? 0 : (float)(cooldownTimeEnd - Time.time);
    }

    public int GetDamageBonus()
    {
        ItemSlot slot = slots[selection];
        if (slot.amount > 0 && slot.item.data is WeaponItem)
            return ((WeaponItem)slot.item.data).damage;
        return 0;
    }
    public int GetDefenseBonus()
    {
        return 0;
    }

    public UsableItem GetUsableItemOrHands(int index)
    {
        ItemSlot slot = slots[index];
        return slot.amount > 0 ? (UsableItem)slot.item.data : hands;
    }

    // Client only //
    void TryUseItem(UsableItem itemData)
    {
        // repeated or one time use while holding mouse down?
        if (itemData.keepUsingWhileButtonDown || Input.GetMouseButtonDown(0))
        {
            Vector3 lookAt = look.lookPositionRaycasted;

            Usability usability = itemData.CanUse(this, selection, lookAt);
            if (usability == Usability.Usable)
            {
                // attack by using the weapon item
                CmdUseItem(selection, lookAt);

                OnUsedItem(itemData, lookAt);
            }
        }
    }


    // Server //
    public void CmdUseItem(int index, Vector3 lookAt)
    {
        if (0 <= index && index < slots.Count && health.current > 0)
        {
            // use item at index, or hands
            UsableItem itemData = GetUsableItemOrHands(index);
            if (itemData.CanUse(this, index, lookAt) == Usability.Usable)
            {
                // use it
                itemData.Use(this, index, lookAt);

                // reset usage time on server
                usageEndTime = Time.time + itemData.cooldown;

                RpcUsedItem(new Item(itemData).hash, lookAt);
            }
        }
    }

    // Client RPC command //
    public void RpcUsedItem(int itemNameHash, Vector3 lookAt)
    {
        // if (isLocalPlayer) return
        Item item = new Item { hash = itemNameHash };

        OnUsedItem((UsableItem)item.data, lookAt);
    }

    // used by local simulation and Rpc, so we might as well put it in a function
    void OnUsedItem(UsableItem itemData, Vector3 lookAt)
    {
        //if (isLocalPlayer)
        usageEndTime = Time.time + itemData.cooldown;

        itemData.OnUsed(this, lookAt);

        // Animation can be done here since we have the animator
    }

    void Update()
    {
        // localplayer selected item usage
        //if (!isLocalPlayer) return;
        if (Input.GetMouseButton(0) &&
            Cursor.lockState == CursorLockMode.Locked &&
            health.current > 0 &&
            CoolDownTimeRemaining() == 0 &&
            !Utils.IsCursorOverUserInterface())
        {
            // use current item or hands
            TryUseItem(GetUsableItemOrHands(selection));
        }
    }


}