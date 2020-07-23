using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public virtual void Interact(Creature creature) { }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
            Interact(other.GetComponentInParent<Creature>());
    }
}