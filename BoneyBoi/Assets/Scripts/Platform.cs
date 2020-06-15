using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.position.y + 0.05f < transform.position.y || Input.GetKey(KeyCode.S))
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        else
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }
}