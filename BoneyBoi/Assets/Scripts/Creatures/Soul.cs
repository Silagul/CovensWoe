using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : Creature
{
    void Start()
    {
        AI = new SoulAI();
    }
}