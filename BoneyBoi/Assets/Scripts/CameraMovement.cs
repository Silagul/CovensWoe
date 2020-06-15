using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 lookat = Vector2.zero;
    void Update()
    {
        transform.position = (Vector3)lookat - new Vector3(0, 0, 10);
    }
}