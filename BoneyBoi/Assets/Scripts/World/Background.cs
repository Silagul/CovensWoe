using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    Vector3 worldPosition;
    [Range(-10.0f, 10.0f)]
    public float distance = 0.0f;
    float movementWeight;

    void Start()
    {
        worldPosition = transform.position;
        movementWeight = (distance / 10.0f);
    }

    void Update()
    {
        float localX = (Camera.main.transform.position.x - worldPosition.x) * movementWeight;
        transform.position = worldPosition + new Vector3(localX, 0);
    }
}