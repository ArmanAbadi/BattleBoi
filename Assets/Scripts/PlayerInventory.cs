using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public static class PlayerInventory
{
    public static Dictionary<ItemName, Item> Items = new Dictionary<ItemName, Item>();
    public static ItemName[] InventoryItems = new ItemName[GlobalConstants.MaxBagSize];
    public static void AddItem(Item Item)
    {
        if (Items.ContainsKey(Item.itemName))
        {
            Items[Item.itemName].itemCount++;
        }
        else
        {
            for (int i = 0; i < InventoryItems.Length; i++)
            {
                if (InventoryItems[i] == ItemName.none)
                {
                    InventoryItems[i] = Item.itemName;
                    Items.Add(Item.itemName, Item);
                    Items[Item.itemName].itemCount = 1;
                    break;
                }
            }
        }
        ItemBarController.Instance.RefreshItems();
        PlayerBag.Instance.RefreshItems();
    }
    public static void UseItem(Item Item)
    {
        if (Items.ContainsKey(Item.itemName))
        {
            Items[Item.itemName].itemCount--;
            if (Items[Item.itemName].itemCount <= 0)
            {
                for (int i = 0; i < InventoryItems.Length; i++)
                {
                    if (InventoryItems[i] == Item.itemName)
                    {
                        Items.Remove(Item.itemName);
                        InventoryItems[i] = ItemName.none;
                        break;
                    }
                }
            }
        }
        ItemBarController.Instance.RefreshItems();
        PlayerBag.Instance.RefreshItems();
    }
}