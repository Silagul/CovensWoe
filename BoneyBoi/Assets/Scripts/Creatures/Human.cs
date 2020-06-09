using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Creature
{
    void Start()
    {
        AI = new HumanAI();
    }

    //Human AI can be possessed
    public override bool Possess()
    {
        if (!isActive)
        {
            Activate();
            tag = "Player";
            AI = new HumanAI();
            return true;
        }
        return false;
    }
}