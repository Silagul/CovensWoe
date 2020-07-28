using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endscreen : MonoBehaviour
{
    public float startPosition, endPosition;
    public Color32 color;
    float difference;
    float startVolume;
    SpriteRenderer spriteRenderer;
    Transform cameraTransform;

    void Start()
    {
        difference = endPosition - startPosition;
        spriteRenderer = GetComponent<SpriteRenderer>();
        cameraTransform = Camera.main.transform;
        startVolume = AudioListener.volume;
    }
    
    void Update()
    {
        float value = (cameraTransform.position.x - startPosition) / difference;
        spriteRenderer.color = Color32.Lerp(new Color32(color.r, color.b, color.g, 0), new Color32(color.r, color.b, color.g, 255), value);
        AudioListener.volume = Mathf.Lerp(startVolume, 0, value);
        transform.position = new Vector3(transform.position.x, cameraTransform.position.y, 0);
        if (spriteRenderer.color.a == 1.0f)
            EndGame.instance.SetActive();
    }
}
