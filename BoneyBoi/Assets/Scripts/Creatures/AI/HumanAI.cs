using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanAI : BaseAI
{
    float speed = 4.0f;
    float vertical = 0.0f;
    float horizontal = 0.0f;
    float acceleration = 8.0f;
    float delay = 0.0f; //timer for death and possession delay etc...

    public override void Movement(Creature creature)
    {
        float horizontalGoal = 0.0f;
        if (creature.isActive)
        {
            if (Input.GetKey(KeyCode.D)) { horizontalGoal += speed; }
            if (Input.GetKey(KeyCode.A)) { horizontalGoal -= speed; }
        }
        horizontal = Mathf.Lerp(horizontal, horizontalGoal, (acceleration * Time.fixedDeltaTime) / Mathf.Abs(horizontal - horizontalGoal));

        if (creature.CollidesWith("Floor"))
            if (Input.GetKey(KeyCode.Space) && creature.isActive) { vertical = 9.81f; }
            else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); }
        else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); }
        creature.transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
    }

    public override void Action(Creature creature)
    {
        if (creature.isActive)
        {
            delay += Time.deltaTime;
            if (delay > 1.0f && Input.GetKey(KeyCode.E))
            {
                creature.Retreat(); //Leave body behind
                GameObject soul = Object.Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), creature.transform.position, Quaternion.identity);
            }
        }
    }
}