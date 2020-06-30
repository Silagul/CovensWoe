using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 worldPosition;
    [Range(-100.0f, 100.0f)]
    public float distance = 0.0f;
    float movementWeight;
    void Start()
    {
        worldPosition = transform.position;
        movementWeight = 1.0f - (distance / 100.0f);
    }

    void Update()
    {
        Vector2 localPosition = (worldPosition - Camera.main.transform.position) * movementWeight;
        transform.position = worldPosition + (Vector3)localPosition;
    }
}