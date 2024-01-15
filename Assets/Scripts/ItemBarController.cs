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

    Dictionary<ItemName, ItemButton> ItemButtons = new Dictionary<ItemName, ItemButton>();
    
    public void RefreshItems()
    {
        foreach (var item in PlayerInventory.Items)
        {
            if (ItemButtons.ContainsKey(item.Key.itemName))
            {
                ItemButtons[item.Key.itemName].SetCount(item.Value);
                if (item.Value <= 0)
                {
                    Destroy(ItemButtons[item.Key.itemName].gameObject);
                    ItemButtons.Remove(item.Key.itemName);
                }
            }
            else
            {
                if (item.Value > 0)
                {
                    GameObject itemButton = Instantiate(ItemButtonPrefab, transform);
                    ItemButtons.Add(item.Key.itemName, itemButton.GetComponent<ItemButton>());
                    itemButton.GetComponent<ItemButton>().SetItemButton(item.Key, item.Value);
                }
            }
        }
    }
}
