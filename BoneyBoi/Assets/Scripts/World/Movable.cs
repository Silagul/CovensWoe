﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Interactable
{
    public static List<Movable> movables = new List<Movable>();
    public bool lookRight;
    public Vector3 offset;
    public float vertical = 0.0f;
    public bool isDragging = false;
    public AudioClip boxMovingAudio;

    void Awake()
    {
        movables.Add(this);
    }

    private void OnDestroy()
    {
        movables.Remove(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Creature creature = other.GetComponent<Creature>();
            if (isDragging)
            {
                isDragging = false;
                creature.transform.GetChild(0).gameObject.SetActive(false);
                creature.GetComponent<Animator>().SetBool("Grappling", false);
                creature.GetComponent<Skeleton>().canRotate = true;
                creature.GetComponent<Animator>().speed = 1.0f;
            }
        }
    }

    //Use to move boxes to avoid errors in movement.
    public void Movement(Vector3 movement)
    {
        List<Vector3> childPositions = new List<Vector3>();
        foreach (Transform child in transform.parent)
            childPositions.Add(child.position);
        Vector3 next = movement;
        transform.parent.gameObject.layer = LayerMask.GetMask("Ignore Raycast");
        LayerMask mask = LayerMask.GetMask("Default");
        RaycastHit2D hit = Physics2D.Raycast(next + new Vector3(0, 0.5f), Vector2.down, 3.7f, mask);
        if (hit)
        {
            next = new Vector3(hit.point.x, hit.point.y + 1.0f, movement.z);
            GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        }
        else
            GetComponentInParent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
        transform.parent.gameObject.layer = LayerMask.GetMask("Default");
        transform.parent.position = next;
        for (int i = 0; i < childPositions.Count; i++)
            if (transform.parent.GetChild(i).tag == "Player" && Input.GetKey(KeyCode.Q))
                transform.parent.GetChild(i).position = childPositions[i];
    }

    public override void Interact(Creature creature)
    {
        GameObject floor = creature.CollidesWith("Floor");
        if (floor != transform.parent.gameObject)
        {
            Skeleton skeleton = creature.GetComponent<Skeleton>();
            bool dragTest = false;
            foreach (Movable movable in movables)
                if (movable != this && movable.isDragging)
                { dragTest = true; break; }
            if (skeleton != null && !dragTest)
            {
                if (Input.GetKey(KeyCode.Q) && GetComponentInParent<Platform>().floorCount != 0)
                {
                    isDragging = true;
                    skeleton.canRotate = false;
                    creature.transform.GetChild(0).gameObject.SetActive(true);
                    if (lookRight) creature.transform.localScale = new Vector3(-0.15f, 0.15f, 1);
                    else creature.transform.localScale = new Vector3(0.15f, 0.15f, 1);
                    creature.GetComponent<Animator>().SetBool("Grappling", true);
                    Vector2 nextPosition = creature.transform.position + offset;
                    Movement(nextPosition);
                    if (skeleton.horizontal != 0)
                        AudioManager.CreateAudio(boxMovingAudio, false, false, this.transform);
                }
                else
                {
                    isDragging = false;
                    creature.transform.GetChild(0).gameObject.SetActive(false);
                    creature.GetComponent<Animator>().SetBool("Grappling", false);
                    creature.GetComponent<Skeleton>().canRotate = true;
                    creature.GetComponent<Animator>().speed = 1.0f;
                }
            }
        }
    }
}