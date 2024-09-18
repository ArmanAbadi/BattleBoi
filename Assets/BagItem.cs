using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GlobalConstants;

public class BagItem : MonoBehaviour
{
    public Image itemImage;
    public TextMeshProUGUI itemCount;
    public Item Item;

    public void SetItem(Item item)
    {
        Item = item;
        itemImage.sprite = item.itemSprite;
        itemCount.text = item.Quantity.ToString();
        GetComponent<Drag>().CanvasTop = PlayerBag.Instance.CanvasTop;
        GetComponent<Drag>().CanvasBot = PlayerBag.Instance.CanvasBot;
    }
    public void SetCount(int value)
    {
        itemCount.text = value.ToString();
    }
}
