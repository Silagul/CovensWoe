using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 worldPosition;
    float movementWeight;
    void Start()
    {
        worldPosition = transform.position;
        movementWeight = 1.0f - ((worldPosition.z + 10.0f) / 10.0f);
    }

    void Update()
    {
        Vector2 localPosition = (worldPosition - Camera.main.transform.position) * movementWeight;
        transform.position = worldPosition + (Vector3)localPosition;
    }
}