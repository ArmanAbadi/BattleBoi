using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static GlobalConstants;

public class Drop : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Drag draggable = eventData.pointerDrag.GetComponent<Drag>();
        if (draggable != null)
        {
            draggable.startPosition = transform.position;
            Item item = draggable.GetComponent<BagItem>().Item;

            for(int i = 0; i < PlayerInventory.InventoryItems.Length ; i++)
            {
                if(item.itemName == PlayerInventory.InventoryItems[i])
                {
                    PlayerInventory.InventoryItems[i] = ItemName.none;
                    PlayerInventory.InventoryItems[transform.GetSiblingIndex()] = item.itemName;
                    break;
                }
            }
        }
    }
}
