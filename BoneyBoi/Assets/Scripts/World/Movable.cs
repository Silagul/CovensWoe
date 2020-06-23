using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Interactable
{
    bool isDragging = false;
    public Vector3 offset;
    public float vertical = 0.0f;
    public void FixedUpdate()
    {
        if (!isDragging & GetComponentInParent<Platform>().floorCount == 0)
            vertical = Mathf.Max(vertical - 9.81f * Time.fixedDeltaTime, -9.81f);
        else
            vertical = 0.0f;
        transform.parent.position += new Vector3(0, vertical * Time.fixedDeltaTime, 0);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "Skeleton")
            Interact(other.GetComponent<Creature>());
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (vertical < 0.0f && collision.tag == "Player")
            collision.GetComponent<Creature>().SetState("Dead");
    }

    //Use to move boxes to avoid errors in movement.
    public void Movement(Vector3 movement)
    {
        List<Vector3> childPositions = new List<Vector3>();
        foreach (Transform child in transform.parent)
            childPositions.Add(child.position);
        transform.parent.position = movement;
        for (int i = 0; i < childPositions.Count; i++)
            if (transform.parent.GetChild(i).tag == "Player" && Input.GetKey(KeyCode.Q))
                transform.parent.GetChild(i).position = childPositions[i];
    }

    public override void Interact(Creature creature)
    {
        if (Input.GetKey(KeyCode.Q) && creature.CollidesWith("Floor")?.gameObject != transform.parent.gameObject)
        {
            isDragging = true;
            Movement(creature.transform.position + offset);
        }
        else isDragging = false;
    }
}