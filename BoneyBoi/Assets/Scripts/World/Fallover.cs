using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallover : MonoBehaviour
{
    bool active = false;
    float time = 0.0f;
    float minRotation;
    public float maxRotation;
    void Start() { minRotation = transform.eulerAngles.z; }
    public void Fall()
    {
        active = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) active = true;
        if (active)
        {
            time += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(minRotation, maxRotation, time));
            if (time >= 1.0f)
                active = false;
        }
    }
}