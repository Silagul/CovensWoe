using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    //Use to move boxes to avoid errors in movement.
    public void Movement(Vector3 movement)
    {
        List<Vector3> childPositions = new List<Vector3>();
        foreach (Transform child in transform)
            childPositions.Add(child.position);
        transform.parent.position = movement;
        for (int i = 0; i < childPositions.Count; i++)
            if (transform.GetChild(i).tag == "Player")
                transform.GetChild(i).position = childPositions[i];
    }

    void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player.transform.position.y + 0.05f < transform.position.y || Input.GetKey(KeyCode.S))
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
        else
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.transform.parent = transform;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.transform.parent = null;
    }

    void OnDestroy()
    {
        while (transform.childCount != 0)
            transform.GetChild(0).parent = null;
    }
}