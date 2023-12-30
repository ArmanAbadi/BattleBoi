using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants
{
    public static string SlashTrigger = "Slash";
    public static string DeadTrigger = "Dead";
    public static string HorizontalVelocity = "HorizontalVelocity";

    public enum Tags
    {
        Player,
        Monster,
        Sword
    }
    public enum ItemType
    {
        Weapon,
        PigMeat
    }
    public class Item
    {
        public ItemType itemType;
        public Sprite ItemSprite;

        public virtual void ActivateItem()
        {
            PlayerInventory.UseItem(this);
        }
    }
    [Serializable]
    public class HealthPotion : Item
    {
        public int HealthGain = 5;
        public HealthPotion()
        {
            itemType = ItemType.PigMeat;
        }
        public override void ActivateItem()
        {
            base.ActivateItem();
            PlayerController.Instance.Heal(HealthGain);

            Debug.Log("use item");
        }
    }
}