using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public static class PlayerInventory
{
    public static Dictionary<Item, int> Items = new Dictionary<Item, int>();
    public static void AddItem(Item Item)
    {
        foreach(var item in Items)
        {
            if(item.Key.itemType == Item.itemType)
            {
                Items[item.Key]++;
                ItemBarController.Instance.RefreshItems();
                return;
            }
        }
        Items.Add(Item, 1);
        ItemBarController.Instance.RefreshItems();
    }
    public static void UseItem(Item Item)
    {
        foreach (var item in Items)
        {
            if (item.Key.itemType == Item.itemType)
            {
                Items[item.Key]--;
                ItemBarController.Instance.RefreshItems();
                return;
            }
        }
    }
}