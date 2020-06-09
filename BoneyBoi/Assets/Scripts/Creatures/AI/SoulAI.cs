using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulAI : BaseAI
{
    Vector2 movement = Vector2.zero;
    float acceleration = 16.0f;
    float speed = 4.0f;
    float delay = 0.0f;

    public override void Movement(Creature creature)
    {
        Vector2 movementGoal = Vector2.zero;
        if (creature.isActive)
        {
            if (Input.GetKey(KeyCode.W)) { movementGoal.y += 1.0f; }
            if (Input.GetKey(KeyCode.D)) { movementGoal.x += 1.0f; }
            if (Input.GetKey(KeyCode.S)) { movementGoal.y -= 1.0f; }
            if (Input.GetKey(KeyCode.A)) { movementGoal.x -= 1.0f; }
        }
        movementGoal = movementGoal.normalized * speed;
        float difference = Vector2.Distance(movement, movementGoal);
        movement = Vector2.Lerp(movement, movementGoal, (acceleration * Time.fixedDeltaTime) / difference);
        creature.transform.position += (Vector3)(movement * Time.fixedDeltaTime);
    }

    //Soul can Possess "Hollow" targets and dissipate
    bool toDestroy = false;
    public override void Action(Creature creature)
    {
        delay += Time.deltaTime;
        if (toDestroy)
        {
            creature.transform.localScale = Vector3.Slerp(Vector3.one, Vector3.zero, delay);
            if (delay >= 1.0f)
                Object.Destroy(creature.gameObject);
        }
        else if (creature.isActive)
        {
            GameObject target;
            if (delay > 1.0f && Input.GetKey(KeyCode.E) && (target = creature.CollidesWith("Hollow")) != null && target.GetComponent<Creature>().Possess())
            {
                delay = 0.0f;
                toDestroy = true;
                creature.Deactivate();
            }
        }
    }
}