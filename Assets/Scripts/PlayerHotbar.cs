using UnityEngine;
using UnityEngine.UI;
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

    [HideInInspector]
    public int selection = 0;

    [HideInInspector]
    public double usageEndTime;

    private double cooldownTimeEnd;

    public MeleeWeaponItem hands;
    public Transform weaponMount;

    public Slider healthUI;

    void Start()
    {
        for (int i = 0; i < size; i++)
        {
            if (i < defaultItems.Length)
            {
                slots.Add(new ItemSlot(new Item(defaultItems[i])));
            }
            else
            {
                slots.Add(new ItemSlot());
            }

        }

    }

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
    public void CmdSelect(int index)
    {
        if (0 <= index && index < slots.Count)
        {
            selection = index;
        }
    }

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
        if (itemData is WeaponItem)
        {
            WeaponItem weapon = (WeaponItem)itemData;
            animator.SetBool(weapon.animationParameter, true);
        }


    }

    public void OnDamageReceived(GameObject attacker, int damageDealt)
    {
        combat.baseDefense -= damageDealt / 2;
    }

    private void ResetAnimation(UsableItem itemData, Vector3 lookAt)
    {
        if (itemData is WeaponItem)
        {
            WeaponItem weapon = (WeaponItem)itemData;

            Usability usability = weapon.CanUse(this, selection, lookAt);

            if (usability == Usability.Usable)
            {
                animator.SetBool(weapon.animationParameter, false);
            }
        }
    }

    public void RefreshLocation(Transform location, ItemSlot slot)
    {
        // valid item, not cleared?
        if (slot.amount > 0)
        {
            ScriptableItem itemData = slot.item.data;
            // new model? (don't do anything if it's the same model, which
            // happens after only Item.ammo changed, etc.)
            // note: we compare .name because the current object and prefab
            // will never be equal
            if (location.childCount == 0 || itemData.modelPrefab == null ||
                location.GetChild(0).name != itemData.modelPrefab.name)
            {
                // delete old model (if any)
                if (location.childCount > 0)
                    Destroy(location.GetChild(0).gameObject);

                // use new model (if any)
                if (itemData.modelPrefab != null)
                {
                    // instantiate and parent
                    GameObject go = Instantiate(itemData.modelPrefab);
                    go.name = itemData.modelPrefab.name; // avoid "(Clone)"
                    go.transform.SetParent(location, false);

                }
            }
        }
        else
        {
            // empty now. delete old model (if any)
            if (location.childCount > 0)
                Destroy(location.GetChild(0).gameObject);
        }
    }

    void Update()
    {
        RefreshLocation(weaponMount, slots[selection]);
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
        else if (health.current <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            GetComponent<CapsuleCollider>().enabled = false;
        }
        ResetAnimation(GetUsableItemOrHands(selection), look.lookPositionRaycasted);
    }


}