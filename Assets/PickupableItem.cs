using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public class PickupableItem : MonoBehaviour
{
    public Resource resource;
    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = resource.ItemSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            PlayerInventory.AddItem(resource);
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
