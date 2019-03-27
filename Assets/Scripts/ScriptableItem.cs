using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/ScriptableItem", order = 999)]
public class ScriptableItem : ScriptableObject
{
    [Header("Base Stats")]
    public int maxStack = 1; // set default already
    public int maxDurability = 1000; // all items should have durability
    public bool destroyable;
    [TextArea(1, 30)] public string toolTip;
    public Sprite sprite;

    [Header("3D Representation")]
    public GameObject modelPrefab; // shown when equipped/in hands/etc.
    public ItemDrop drop;

    static Dictionary<int, ScriptableItem> cache;
    public static Dictionary<int, ScriptableItem> dict
    {
        get
        {
            if (cache == null)
            {
                cache = Resources.LoadAll<ScriptableItem>("").ToDictionary(
                item => item.name.GetStableHashCode(), item => item);

                foreach (int key in cache.Keys)
                {
                    Debug.Log(key);
                }

                return cache;
            }

            return cache;
        }
    }

}
