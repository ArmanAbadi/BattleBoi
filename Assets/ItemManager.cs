using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public class ItemManager : MonoBehaviour
{
    [SerializeField]
    public Item[] baseItems;
    [SerializeField]
    public HealthPotion[] healthPotions;
    [SerializeField]
    public Resource[] resourceItems;

    public List<Item> PlayerItems = new List<Item> { };

    public static ItemManager Instance;

    private void Start()
    {
        if (Instance == null) Instance = this;

        foreach (var Item in baseItems)
        {
            PlayerItems.Add(Item);
        }
        foreach (var Item in healthPotions)
        {
            PlayerItems.Add(Item);
        }
        foreach (var Item in resourceItems)
        {
            PlayerItems.Add(Item);
        }
    }

    public Item GetItemByItemname(ItemName itemName)
    {
        foreach(var Item in PlayerItems)
        {
            if(Item.itemName == itemName) return Item;
        }
        return null;
    }
    public void SetItemQuantities(List<serverItem> items)
    {
        foreach (serverItem item in items)
        {
            SetItemQuantity(item);
        }
    }
    public void SetItemQuantity(serverItem item)
    {
        foreach (var Item in PlayerItems)
        {
            if (Item.itemName.ToString() == item.itemName) Item.Quantity = item.Quantity;
        }
    }
}
public enum ItemType
{
    Equippable,
    Consumable,
    Resource
}
public enum ItemName
{
    none,
    PigMeat,
    IronOre,
    Stick
}
[Serializable]
public class Item
{
    public ItemName itemName;
    public ItemType itemType;
    public Sprite itemSprite;
    public int Quantity = 0;
    public GameObject prefab;
    public virtual void ActivateItem()
    {
        PlayerInventory.UseItem(this);
    }
}
[Serializable]
public class HealthPotion : Item
{
    public int HealthGain = 5;
    public HealthPotion()
    {
        itemType = ItemType.Consumable;
    }
    public override void ActivateItem()
    {
        if (PlayerController.Instance.IsFullHealth()) return;
        base.ActivateItem();
        PlayerController.Instance.Heal(HealthGain);
    }
}
[Serializable]
public class Resource : Item
{
    public Resource()
    {
        itemType = ItemType.Resource;
    }
    public override void ActivateItem()
    {
    }
}