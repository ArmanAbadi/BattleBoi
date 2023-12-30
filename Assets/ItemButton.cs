using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalConstants;

public class ItemButton : MonoBehaviour
{
    Image Image;
    TextMeshProUGUI ItemCountText;
    ItemType itemType;
    int count = 0;
    int Count
    {
        get { return count; }
        set
        {
            count = value;
            ItemCountText.text = count.ToString();
        }
    }

    public void SetItemButton(Item item, int itemCount)
    {
        Image = GetComponent<Image>();
        ItemCountText = GetComponentInChildren<TextMeshProUGUI>();
        
        itemType = item.itemType;
        Image.sprite  = item.ItemSprite;
        ItemCountText.text = itemCount.ToString();
        count = itemCount;

        GetComponent<Button>().onClick.AddListener(item.ActivateItem);
    }
    public void SetCount(int value)
    {
        Count = value;
    }
}
