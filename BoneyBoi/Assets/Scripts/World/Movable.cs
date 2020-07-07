using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Interactable
{
    public Vector3 offset;
    public float vertical = 0.0f;

    public AudioClip boxMovingAudio;
    public AudioClip boxFallingAudio;

    public void FixedUpdate()
    {
        if (GetComponentInParent<Platform>().floorCount == 0)
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
        if (vertical < 0.0f && collision.GetComponent<Creature>() != null && transform.position.y > collision.transform.position.y + 2.0f)
            collision.GetComponent<Creature>().SetState("Dead");

        if (collision.tag == "Floor")
        {
            AudioManager.CreateAudio(boxFallingAudio, false, this.transform);
        }
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
        GameObject floor = creature.CollidesWith("Floor");
        if (Input.GetKey(KeyCode.Q) && floor?.gameObject != transform.parent.gameObject && GetComponentInParent<Platform>().floorCount != 0)
        {
            offset.x = -creature.transform.localScale.x;
            Movement(creature.transform.position + offset);
            AudioManager.CreateAudio(boxMovingAudio, false, this.transform);
        }
    }
}