using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    public float startPosition, endPosition;
    public Color32 color;
    float difference;
    SpriteRenderer spriteRenderer;
    Transform cameraTransform;

    void Start()
    {
        difference = endPosition - startPosition;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraTransform = Camera.main.transform;
    }
    
    void Update()
    {
        spriteRenderer.color = Color32.Lerp(new Color32(color.r, color.b, color.g, 0), new Color32(color.r, color.b, color.g, 255), (cameraTransform.position.x - startPosition) / difference);
        transform.position = new Vector3(transform.position.x, cameraTransform.position.y, 0);
    }
}
