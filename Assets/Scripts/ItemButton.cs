using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalConstants;

public class ItemButton : MonoBehaviour
{
    public Image Image;
    public TextMeshProUGUI ItemCountText;
    public Button button;
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

    public void SetItemButton(Item item)
    {
        itemType = item.itemType;
        Image.sprite  = item.itemSprite;
        Count = item.Quantity;

        button.onClick.AddListener(item.ActivateItem);
    }
    public void SetCount(int value)
    {
        Count = value;
    }
}
