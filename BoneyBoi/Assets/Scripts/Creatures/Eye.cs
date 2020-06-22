using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    float time;
    float duration;
    Vector2 lookat;
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(0.0f, 360.0f));
        duration = Random.Range(0.5f, 1.0f);
        time = Random.Range(0.0f, duration);
        lookat = Random.insideUnitCircle.normalized * 0.1f;
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time >= duration)
        {
            time -= duration;
            duration = Random.Range(0.5f, 1.0f);
            lookat = Random.insideUnitCircle.normalized * 0.1f;
        }

        Vector2 localDirection = transform.InverseTransformDirection(GameObject.FindGameObjectWithTag("Player").transform.position - transform.position);
        if (Camera.main.transform.position.y > 4.0f)
        {
            localDirection.Normalize();
            transform.GetChild(0).localPosition = localDirection * 0.1f;
        }
        else
            transform.GetChild(0).localPosition = lookat;
    }
}
