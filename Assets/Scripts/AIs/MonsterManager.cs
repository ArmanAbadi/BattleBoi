using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterManager
{
    public static List<PigController> Pigs = new List<PigController>();

    public static void AddPig(PigController Pig)
    {
        Pigs.Add(Pig);
    }
    public static void RemovePig(PigController Pig)
    {
        Pigs.Remove(Pig);
    }
}
