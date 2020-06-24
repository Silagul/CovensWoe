using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backforth : MonoBehaviour
{
    System.Action fadein;
    public bool isVisible = true;
    public bool isActive = false;
    float time = 0.0f;
    public float speed = 2.0f;
    Vector2 startPoint;
    public Vector2 endPoint;
    public Vector2 nextPoint;

    void Start()
    {
        startPoint = transform.position;
        nextPoint = endPoint;
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            time += Time.fixedDeltaTime;
            fadein?.Invoke();

            Vector2 direction = (nextPoint - (Vector2)transform.position);
            Vector2 movement = direction.normalized * speed * Time.fixedDeltaTime;
            if (movement.magnitude >= direction.magnitude - 0.01f)
            {
                movement = direction.normalized * movement.magnitude;
                {
                    if (nextPoint == startPoint) { nextPoint = endPoint; }
                    else { nextPoint = startPoint; }
                }
            }
            transform.position += (Vector3)movement;
        }
    }

    public void Activate(bool active)
    {
        if (active)
        {
            isActive = true;
            if (!isVisible)
            {
                isVisible = true;
                fadein = FadeIn;
            }
        }
        else
        {
            isActive = false;
        }
    }

    void FadeIn()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)(Mathf.Min(time, 1.0f) * 255.0f));
        if (time > 1.0f)
            fadein = null;
    }
}