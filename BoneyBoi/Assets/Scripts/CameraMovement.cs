using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 lookat = Vector2.zero;
    void Start()
    {
        transform.GetChild(0).localScale = new Vector3(Screen.width / 64.0f, Screen.height / 64.0f, 1);
    }

    void Update()
    {
        transform.position = (Vector3)lookat - new Vector3(0, 0, 10);
    }
}