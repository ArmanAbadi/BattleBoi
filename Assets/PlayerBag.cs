using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public class PlayerBag : MonoBehaviour
{
    public GameObject ItemPrefab;
    public GameObject GridObject;
    public Canvas CanvasTop;
    public Canvas CanvasBot;
    public static PlayerBag Instance { get; private set; }

    List<GameObject> items = new List<GameObject>();
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
    private void OnEnable()
    {
        RefreshItems();
    }
    public void RefreshItems()
    {
        Debug.Log("Refresh");
        ClearItems();
        for (int i = 0; i < GlobalConstants.MaxBagSize; i++)
        {
            if(PlayerInventory.InventoryItems[i] != ItemName.none)
            {
                GameObject bagItem = Instantiate(ItemPrefab);
                bagItem.GetComponent<BagItem>().SetItem(PlayerInventory.Items[PlayerInventory.InventoryItems[i]]);
                bagItem.transform.SetParent(CanvasBot.transform, true);
                bagItem.transform.position = GridObject.transform.GetChild(i).position;

                items.Add(bagItem);
            }
        }
    }
    void ClearItems()
    {
        for(int i = items.Count - 1; i >= 0; i--)
        {
            Destroy(items[i]);
            items.RemoveAt(i);
        }
    }
}
