using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    bool isActive = false;
    float time = 0.0f;
    void Update()
    {
        time += Time.deltaTime;
    }

    public override void Interact(Creature creature)
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (time > 1.0f)
            {
                time = 0.0f;
                isActive = !isActive;
                if (isActive) transform.parent.Find("PlatformMoving")?.GetComponent<Backforth>().Activate(true);
                else transform.parent.Find("PlatformMoving")?.GetComponent<Backforth>().Activate(false);
            }
        }
    }
}