using UnityEngine;
using System.Collections.Generic;

public struct Item
{

    public int hash;

    // ammo for weapon items
    public int ammo;

    // current durability
    public int durability;

    public int NULL;

    public Item(ScriptableItem data)
    {
        hash = data.name.GetStableHashCode();
        ammo = 0;
        durability = data.maxDurability;
        NULL = 0;
    }

    public ScriptableItem data
    {
        get
        {
            if (!ScriptableItem.dict.ContainsKey(hash))
                throw new KeyNotFoundException("There is no ScriptableItem with hash=" + hash);
            return ScriptableItem.dict[hash];
        }
    }

    public int maxStack { get { return data.maxStack; } }

    //public Sprite sprite {get {return }};
    public Sprite sprite { get { return data.sprite; } }

}