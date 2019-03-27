using UnityEngine;
using UnityEngine.UI;

public class UIHotbar : MonoBehaviour
{
    public GameObject panel;
    public UIHotbarSlot slotPrefab;
    public Transform content;

    public GameObject player; // for now

    void Update()
    {
        if (player)
        {
            panel.SetActive(true);

            PlayerHotbar hotbar = player.GetComponent<PlayerHotbar>();
            PlayerLook look = player.GetComponent<PlayerLook>();

            //Utils.BalancePrefabs(slotPrefab.gameObject, hotbar.size, content);

            for (int i = 0; i < hotbar.size; i++)
            {

                UIHotbarSlot slot = content.GetChild(i).GetComponent<UIHotbarSlot>();

                ItemSlot itemSlot = hotbar.slots[i];

                if (Input.GetKeyDown(hotbar.keys[i]))
                {
                    if (itemSlot.amount == 0)
                    {
                        hotbar.CmdSelect(i);
                    }
                    else if (itemSlot.item.data is UsableItem)
                    {
                        if (((UsableItem)itemSlot.item.data).useDirectly)
                        {
                            Debug.Log("Can use directly");
                            hotbar.CmdUseItem(i, look.lookPositionRaycasted);
                        }
                        else
                        {
                            Debug.Log("Cannot use directly");
                            hotbar.CmdSelect(i);
                        }
                    }
                    else
                    {
                        Debug.Log("Different beast");
                    }
                }

                slot.hotkeyText.text = hotbar.keys[i].ToString().Replace("Alpha", "");
                slot.selectionOutline.SetActive(i == hotbar.selection);

                if (itemSlot.amount > 0)
                {
                    slot.image.sprite = itemSlot.item.sprite;
                    slot.amountOverlay.SetActive(itemSlot.amount > 1);
                    if (itemSlot.amount > 1) slot.amountText.text = itemSlot.amount.ToString();
                }
                else
                {
                    slot.image.color = Color.clear;
                    slot.image.sprite = null;
                    slot.amountOverlay.SetActive(false);
                }
            }
        }
        else panel.SetActive(false);
    }
}