using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public class PickupableItem : MonoBehaviour
{
    public Resource IronOre;
    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = IronOre.ItemSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            PlayerInventory.AddItem(IronOre);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
