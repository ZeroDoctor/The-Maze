using UnityEngine;

// Future proofing
public class ItemDrop : MonoBehaviour
{
    [SerializeField] ScriptableItem itemData;

    public int amount = 1;

    [HideInInspector]
    public Item item;

    // should happen at the start of the server
    void Start()
    {
        if (item.hash == 0 && itemData != null)
            item = new Item(itemData);
    }

    public void OnInteractClient(GameObject player) { }
    public void OnInteractServer(GameObject player) { }


}