using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

    static Dictionary<int, ScriptableItem> cache;
    public static Dictionary<int, ScriptableItem> dict
    {
        get
        {
            // load if not loaded yet
            return cache ?? (cache = Resources.LoadAll<ScriptableItem>("").ToDictionary(
                item => item.name.GetStableHashCode(), item => item)
            );
        }
    }
}
