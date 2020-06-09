using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI
{
    //Most creatures move, rotate and take action?
    public virtual void Movement(Creature creature) { }
    public virtual void Rotation(Creature creature) { }
    public virtual void Action(Creature creature) { }
}