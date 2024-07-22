using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalConstants;

public class PickupableItem : NetworkBehaviour
{
    public Resource resource;
    public void Start()
    {
        GetComponent<SpriteRenderer>().sprite = resource.itemSprite;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            PlayerInventory.AddItem(resource);
            Runner.Despawn(Object);
        }
    }
}
