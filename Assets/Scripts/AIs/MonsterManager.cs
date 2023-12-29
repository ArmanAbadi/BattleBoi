using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MonsterManager
{
    public static List<PigController> Pigs = new List<PigController>();

    public static void AddPig(GameObject Pig)
    {
        if (Pig.GetComponent<PigController>())
        {
            Pigs.Add(Pig.GetComponent<PigController>());
        }
    }
}
