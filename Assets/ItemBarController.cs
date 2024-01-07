using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public class ItemBarController : MonoBehaviour
{
    public static ItemBarController Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public GameObject ItemButtonPrefab;

    Dictionary<ItemType, ItemButton> ItemButtons = new Dictionary<ItemType, ItemButton>();
    
    public void RefreshItems()
    {
        foreach (var item in PlayerInventory.Items)
        {
            if (ItemButtons.ContainsKey(item.Key.itemType))
            {
                ItemButtons[item.Key.itemType].SetCount(item.Value);
                if (item.Value <= 0)
                {
                    Destroy(ItemButtons[item.Key.itemType].gameObject);
                    ItemButtons.Remove(item.Key.itemType);
                }
            }
            else
            {
                GameObject itemButton = Instantiate(ItemButtonPrefab, transform);
                ItemButtons.Add(item.Key.itemType, itemButton.GetComponent<ItemButton>());
                itemButton.GetComponent<ItemButton>().SetItemButton(item.Key, item.Value);
            }
        }
    }
}
