using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    float time = 0.0f;
    float duration;
    void Start()
    {
        duration = Random.Range(1.0f, 2.0f);
        GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255);
    }

    void Update()
    {
        time += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.zero, time / duration);
        if (time > duration)
            Destroy(gameObject);
    }
}