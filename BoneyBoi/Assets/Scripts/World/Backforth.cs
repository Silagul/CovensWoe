using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backforth : MonoBehaviour
{
    public List<Vector2> stops = new List<Vector2>();

    System.Action fadein;
    public bool isVisible = true;
    public bool isActive = false;
    float time = 0.0f;
    public float speed = 2.0f;
    public bool isDirectionUP;
    public int currentIndex;
    Vector2 nextPoint;

    void Start()
    {
        nextPoint = stops[currentIndex];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Activate(!isActive);
    }

    void FixedUpdate()
    {
        if (isActive)
        {
            time += Time.fixedDeltaTime;
            fadein?.Invoke();

            Vector2 direction = (nextPoint - (Vector2)transform.position);
            Vector2 movement = direction.normalized * speed * Time.fixedDeltaTime;
            if (movement.magnitude >= direction.magnitude)
            {
                movement = direction.normalized * movement.magnitude;
                isActive = false;
            }
            transform.position += (Vector3)movement;
        }
    }

    public void Activate(bool active)
    {
        if (!isActive)
        {
            isActive = true;
            if (!isVisible)
            {
                isVisible = true;
                fadein = FadeIn;
            }
            if (isDirectionUP)
                if (currentIndex == stops.Count - 1)
                {
                    currentIndex--;
                    isDirectionUP = false;
                }
                else
                    currentIndex++;
            else if (currentIndex == 0)
            {
                currentIndex++;
                isDirectionUP = true;
            }
            else
                currentIndex--;
            nextPoint = stops[currentIndex];
        }
    }

    void FadeIn()
    {
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)(Mathf.Min(time, 1.0f) * 255.0f));
        if (time > 1.0f)
            fadein = null;
    }
}