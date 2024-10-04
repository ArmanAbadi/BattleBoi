using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static GlobalConstants;

public class PlayerBag : MonoBehaviour
{
    public GameObject ItemPrefab;
    public GameObject GridObject;
    public Canvas CanvasTop;
    public Canvas CanvasBot;
    public static PlayerBag Instance { get; private set; }
    public GameObject Container;

    List<GameObject> items = new List<GameObject>();

    public UnityEvent PorkChopOwned;

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
    IEnumerator FrameDelay()
    {
        yield return null;

        RefreshItems();
    }
    public void RefreshItems()
    {
        ClearItems();
        for (int i = 0; i < ItemManager.Instance.PlayerItems.Count; i++)
        {
            if (ItemManager.Instance.PlayerItems[i].Quantity >= 1)
            {
                GameObject bagItem = Instantiate(ItemPrefab);
                bagItem.GetComponent<BagItem>().SetItem(ItemManager.Instance.PlayerItems[i]);
                bagItem.transform.SetParent(CanvasBot.transform, true);
                bagItem.transform.position = GridObject.transform.GetChild(i).position;
                bagItem.transform.localScale = Vector3.one;

                items.Add(bagItem);
                
                if (ItemManager.Instance.PlayerItems[i].itemName == ItemName.PigMeat) PorkChopOwned.Invoke();
            }
        }
    }
    public void OpenBag()
    {
        Container.SetActive(true);
        StartCoroutine(FrameDelay());
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
