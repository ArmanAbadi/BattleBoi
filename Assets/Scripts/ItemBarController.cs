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

    List<GameObject> items = new List<GameObject>();

    private void OnEnable()
    {
        RefreshItems();
    }
    public void RefreshItems()
    {
        ClearItems();
        for (int i = 0; i < GlobalConstants.MaxBagSize; i++)
        {
            if (PlayerInventory.InventoryItems[i] != ItemName.none && PlayerInventory.Items[PlayerInventory.InventoryItems[i]].itemType == ItemType.Consumable)
            {
                GameObject itemButton = Instantiate(ItemButtonPrefab, transform);
                itemButton.GetComponent<ItemButton>().SetItemButton(PlayerInventory.Items[PlayerInventory.InventoryItems[i]]);
                items.Add(itemButton);
            }
        }
    }
    void ClearItems()
    {
        for (int i = items.Count - 1; i >= 0; i--)
        {
            Destroy(items[i]);
            items.RemoveAt(i);
        }
    }
}
