using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MudScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Human")
        {
            collision.gameObject.GetComponent<Human>().currentSpeed /= 2;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Human")
        {
            collision.gameObject.GetComponent<Human>().currentSpeed = collision.gameObject.GetComponent<Human>().speed;
        }
    }
}
