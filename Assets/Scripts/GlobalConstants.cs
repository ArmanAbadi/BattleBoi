using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants
{
    public const int MaxBagSize = 25;

    public const string SlashTrigger = "Slash";
    public const string DeadTrigger = "Dead";
    public const string HorizontalVelocity = "HorizontalVelocity";
    public const string VerticalVelocity = "VerticalVelocity";
    public const string Idle = "Idle";
    public const string Attack = "Attack";
    public const string DigUp = "DigUp";
    public const string DigDown = "DigDown"; 
    public const string Aggro = "Aggro";
    public const string HumanAttackUp = "HumanAttackUpNoWeapon";
    public const string HumanAttackDown = "HumanAttackDownNoWeapon";
    public const string HumanAttackLeft = "HumanAttackLeftNoWeapon";
    public const string HumanAttackRight = "HumanAttackRightNoWeapon";

    public enum Tags
    {
        Player,
        Monster,
        Sword,
        Dropable
    }
    public enum ItemType
    {
        Equippable,
        Consumable,
        Resource
    }
    public enum ItemName
    {
        none,
        PigMeat,
        IronOre,
        Stick
    }
    public class Item
    {
        public ItemName itemName;
        public ItemType itemType;
        public Sprite itemSprite;
        public int itemCount = 0;

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
            itemType = ItemType.Consumable;
        }
        public override void ActivateItem()
        {
            if (PlayerController.Instance.IsFullHealth()) return;
            base.ActivateItem();
            PlayerController.Instance.Heal(HealthGain);
        }
    }
    [Serializable]
    public class Resource : Item
    {
        public Resource()
        {
            itemType = ItemType.Resource;
        }
        public override void ActivateItem()
        {
        }
    }
}