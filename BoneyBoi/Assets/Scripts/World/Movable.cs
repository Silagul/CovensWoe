using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : Interactable
{
    public static List<Movable> movables = new List<Movable>();
    public bool isOnRight;
    public Vector3 offset;
    public float vertical = 0.0f;
    public bool isHeld = false;
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
            Skeleton skeleton;
            if (other.TryGetComponent(out skeleton))
                if (isHeld)
                {
                    isHeld = false;
                    skeleton.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
                    skeleton.GetComponent<Skeleton>().canRotate = true;
                    skeleton.anim.SetBool("Grappling", false);
                    skeleton.anim.speed = 1.0f;
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
            if (transform.parent.GetChild(i).tag == "Player" && Input.GetKey(InputManager.instance.interact))
                transform.parent.GetChild(i).position = childPositions[i];
    }

    public override void Interact(Creature creature)
    {
        if (!creature.CollidesWith("Floor", transform.parent.gameObject))
        {
            Skeleton skeleton;
            if (creature.TryGetComponent(out skeleton) && !HoldOtherThanThis())
            {
                int floorCount = GetComponentInParent<Platform>().floorCount;
                if (floorCount != 0 && LookAtThis(creature) && Input.GetKey(InputManager.instance.interact))
                {
                    isHeld = true;
                    skeleton.canRotate = false;
                    creature.transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = true;
                    if (isOnRight) creature.transform.localScale = new Vector3(0.15f, 0.15f, 1);
                    else creature.transform.localScale = new Vector3(-0.15f, 0.15f, 1);
                    creature.GetComponent<Animator>().SetBool("Grappling", true);
                    Vector2 nextPosition = creature.transform.position + offset;
                    Movement(nextPosition);
                    if (skeleton.horizontal != 0)
                        AudioManager.CreateAudio(boxMovingAudio, false, false, transform);
                }
                else
                {
                    isHeld = false;
                    creature.transform.GetChild(0).gameObject.GetComponent<Collider2D>().enabled = false;
                    creature.GetComponent<Animator>().SetBool("Grappling", false);
                    creature.GetComponent<Skeleton>().canRotate = true;
                    creature.GetComponent<Animator>().speed = 1.0f;
                }
            }
        }
    }

    public static bool IsHolding()
    {
        foreach (Movable movable in movables)
            if (movable.isHeld)
                return true;
        return false;
    }

    bool HoldOtherThanThis()
    {
        foreach (Movable movable in movables)
            if (movable != this && movable.isHeld)
                return true;
        return false;
    }

    bool LookAtThis(Creature creature)
    {
        if (creature.transform.localScale.x > 0) { if (isOnRight) { return true; } }
        else if (!isOnRight) { return true; }
        return false;
    }
}