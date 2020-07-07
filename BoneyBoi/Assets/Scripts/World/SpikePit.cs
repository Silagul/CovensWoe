using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Human")
        {
            collision.gameObject.GetComponent<Animator>().SetBool("Foothold", true);
            collision.gameObject.GetComponent<Human>().SetState("Dead");
        }
    }
}
