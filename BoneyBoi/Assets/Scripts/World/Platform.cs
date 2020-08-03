using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float offsetY;
    public int floorCount = 0;

    void Update()
    {
        foreach (Creature creature in Creature.creatures)
        {
            if (creature != null)
            {
                Skeleton skeleton;
                if (creature.TryGetComponent(out skeleton))
                {
                    bool isDragging = false;
                    if (skeleton.CollidesWith("Movable") != null && Input.GetKey(InputManager.instance.interact)) isDragging = true;
                    Vector3 localPosition = transform.InverseTransformPoint(creature.transform.position);
                    if (localPosition.y + 0.1f < offsetY || (creature.isActive && Input.GetKey(InputManager.instance.down) && !isDragging))
                    {
                        Physics2D.IgnoreCollision(skeleton.defaultCollider, GetComponent<Collider2D>(), true);
                        Physics2D.IgnoreCollision(skeleton.hollowCollider, GetComponent<Collider2D>(), true);
                        Physics2D.IgnoreCollision(skeleton.transform.GetComponentsInChildren<BoxCollider2D>()[1], GetComponent<Collider2D>(), true);
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(skeleton.defaultCollider, GetComponent<Collider2D>(), false);
                        Physics2D.IgnoreCollision(skeleton.hollowCollider, GetComponent<Collider2D>(), false);
                        Physics2D.IgnoreCollision(skeleton.transform.GetComponentsInChildren<BoxCollider2D>()[1], GetComponent<Collider2D>(), false);
                    }
                }
                else
                {
                    Human human = creature.GetComponent<Human>();
                    Vector3 localPosition = transform.InverseTransformPoint(creature.transform.position);
                    if (localPosition.y + 0.1f < offsetY || (creature.isActive && Input.GetKey(InputManager.instance.down)))
                    {
                        Physics2D.IgnoreCollision(human.defaultCollider, GetComponent<Collider2D>(), true);
                        Physics2D.IgnoreCollision(human.hollowCollider, GetComponent<Collider2D>(), true);
                        Physics2D.IgnoreCollision(human.transform.GetComponentInChildren<BoxCollider2D>(), GetComponent<Collider2D>() , true);  //this is for PopUp sign collider
                        Physics2D.IgnoreCollision(human.transform.GetComponentsInChildren<CapsuleCollider2D>()[1], GetComponent<Collider2D>(), true);   //this is for Monster collider
                    }
                    else
                    {
                        Physics2D.IgnoreCollision(human.defaultCollider, GetComponent<Collider2D>(), false);
                        Physics2D.IgnoreCollision(human.hollowCollider, GetComponent<Collider2D>(), false);
                        Physics2D.IgnoreCollision(human.transform.GetComponentInChildren<BoxCollider2D>(), GetComponent<Collider2D>(), false);  //this is for PopUp sign collider
                        Physics2D.IgnoreCollision(human.transform.GetComponentsInChildren<CapsuleCollider2D>()[1], GetComponent<Collider2D>(), false);  //this is for Monster collider
                    }
                }


            }
        }
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Creature"))
            collision.transform.parent = transform;
        else if (collision.gameObject.tag == "Floor")
            floorCount++;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Creature"))
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