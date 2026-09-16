using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    public string Item_Name;
    public string Item_ID;
    public string Item_Image_Name;
}
public class Gem: Item{
    public Gem_Color gem_color;
    public Gem_Modifier[] gem_modifiers;
    public Equipment_Slot equipment_slot;
}
public enum Gem_Color{
    green,
    red,
    yellow,
    blue,
    dark_red,
    grey,
    orange,
    pink,
    purple,
    white
}
public enum Gem_Type{
    health,
    damage,
    move_speed,
    slow,
    aoe,
    armor,
    fire_rate,
    multi_shot,
    penetration,
}
public class Gem_Modifier{
    public Gem_Type gem_type;
    public float gem_modifier_value;
}
public enum Equipment_Slot{
    Health,
    Armor,
    Weapon
}