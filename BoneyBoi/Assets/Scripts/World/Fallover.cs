using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fallover : Interactable
{
    bool active = false;
    float time = 0.0f;
    float minRotation;
    public float maxRotation;
    void Start() { minRotation = transform.eulerAngles.z; }
    public override void Interact(Creature creature)
    {
        tag = "Floor";
        gameObject.layer = LayerMask.NameToLayer("Default");
        active = true;
        Platform platform;
        if (TryGetComponent(out platform))
            platform.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O)) Interact(null);
        if (active)
        {
            time += Time.deltaTime;
            transform.eulerAngles = new Vector3(0, 0, Mathf.Lerp(minRotation, maxRotation, time));
            if (time >= 1.0f)
                active = false;
        }
    }

    void OnTriggerStay2D(Collider2D other) { }
}