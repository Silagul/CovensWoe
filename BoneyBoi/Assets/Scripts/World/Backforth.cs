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
        Transform t = transform.GetChild(0);
        float distance = Mathf.Abs(transform.position.y - stops[stops.Count - 1].y);
        t.position = new Vector3(t.position.x, Mathf.Lerp(transform.position.y, stops[stops.Count - 1].y, 0.5f) + 5.0f);
        t.GetComponent<SpriteRenderer>().size = new Vector2(0.245f, distance);
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
                movement = direction;
                isActive = false;
            }
            transform.position += (Vector3)movement;
        }
    }

    public void Activate()
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
        //GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, (byte)(Mathf.Min(time, 1.0f) * 255.0f)); //What dis do?
        if (time > 1.0f)
            fadein = null;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "FallingTree")
            if (isActive)
            {
                isDirectionUP = !isDirectionUP;
                currentIndex += isDirectionUP ? 1 : -1;
                nextPoint = stops[currentIndex];
            }
    }
}