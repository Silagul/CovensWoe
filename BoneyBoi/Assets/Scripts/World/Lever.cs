using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    bool isActive = false;
    float time = 0.0f;

    public AudioClip leverAudio;

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
                AudioManager.CreateAudio(leverAudio, false, true, this.transform);
                if (isActive) transform.parent.Find("PlatformMoving")?.GetComponent<Backforth>().Activate(true);
                else transform.parent.Find("PlatformMoving")?.GetComponent<Backforth>().Activate(false);
            }
        }
    }
}