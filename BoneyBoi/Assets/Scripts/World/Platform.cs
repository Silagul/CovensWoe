using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float offsetY;
    public int floorCount = 0;
    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            if (player.transform.position.y + 0.05f < transform.position.y + offsetY || Input.GetKey(KeyCode.S))
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            else
                Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.transform.parent = transform;
        else if (collision.gameObject.tag == "Floor")
            floorCount++;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.transform.parent = GameManager.world.transform;
        else if (collision.gameObject.tag == "Floor")
            floorCount--;
    }

    void OnDestroy()
    {
        foreach (Transform child in transform)
            if (child.name == "Human")
                child.parent = GameManager.world.transform;
    }
}