using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Secret : MonoBehaviour
{
    System.Action fadeout;
    float time = 0.0f;
    void Update()
    {
        fadeout?.Invoke();
    }

    void Fadeout()
    {
        time += Time.deltaTime;
        GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, (byte)((1.0f - time * 0.5f) * 255.0f));
        if (time > 1.0f)
            fadeout = null;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.name == "Human")
        fadeout = Fadeout;
    }
}