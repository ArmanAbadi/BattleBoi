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
        foreach(var Item in ItemManager.Instance.PlayerItems)
        {
            if (Item.Quantity >= 1 && Item.itemType == ItemType.Consumable)
            {
                GameObject itemButton = Instantiate(ItemButtonPrefab, transform);
                itemButton.GetComponent<ItemButton>().SetItemButton(Item);
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
