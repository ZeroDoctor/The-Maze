using System;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

public struct ItemSlot
{
    public Item item;
    public int amount;

    public ItemSlot(Item item, int amount = 1)
    {
        this.item = item;
        this.amount = amount;
    }

    public int DecreaseAmount(int reduceBy)
    {
        int limit = Mathf.Clamp(reduceBy, 0, amount);
        amount -= limit;
        return limit;
    }

    public int IncreaseAmount(int increaseBy)
    {
        int limit = Mathf.Clamp(increaseBy, 0, item.maxStack - amount);
        amount += limit;
        return limit;
    }
}

public class ListItemSlot : List<ItemSlot> { }