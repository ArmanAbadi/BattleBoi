using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalConstants
{
    public static string SlashTrigger = "Slash";
    public static string DeadTrigger = "Dead";
    public static string HorizontalVelocity = "HorizontalVelocity";
    public static string VerticalVelocity = "VerticalVelocity";
    public static string Idle = "Idle";
    public static string Attack = "Attack";
    public static string DigUp = "DigUp";
    public static string DigDown = "DigDown"; 
    public static string Aggro = "Aggro";
    public static string HumanAttackUp = "HumanAttackUpNoWeapon";
    public static string HumanAttackDown = "HumanAttackDownNoWeapon";
    public static string HumanAttackLeft = "HumanAttackLeftNoWeapon";
    public static string HumanAttackRight = "HumanAttackRightNoWeapon";

    public enum Tags
    {
        Player,
        Monster,
        Sword,
        Dropable
    }
    public enum ItemType
    {
        Weapon,
        HealthPotion,
        Resource
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
            itemType = ItemType.HealthPotion;
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