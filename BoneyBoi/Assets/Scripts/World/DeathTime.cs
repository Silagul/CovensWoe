using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTime : MonoBehaviour
{
    float time = 0.0f;
    float duration;

    public DeathTime SetDuration(float d)
    {
        duration = d;
        return this;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > duration)
            Destroy(gameObject);
    }
}