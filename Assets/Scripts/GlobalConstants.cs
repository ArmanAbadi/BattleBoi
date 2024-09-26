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
    public const string AttackUp = "AttackUp";
    public const string AttackDown = "AttackDown";
    public const string AttackRight = "AttackRight";
    public const string AttackLeft = "AttackLeft";

    public enum Tags
    {
        Player,
        Monster,
        Sword,
        Dropable
    }
}