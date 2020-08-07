using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{
    float time;
    [Range(0.0f, 5.0f)]
    public float duration;
    Vector2 lookat;
    Vector2 offset;
    System.Action update;
    void Start()
    {
        offset = transform.GetChild(0).localPosition;
        time = Random.Range(0.0f, duration);
        lookat = Random.insideUnitCircle.normalized * 0.2f;
        update = WhileActive;
    }

    void Update()
    {
        update.Invoke();
    }

    void WhileActive()
    {
        time += Time.deltaTime;
        if (time >= duration)
        {
            time -= duration;
            update = WhileBlink;
            transform.GetChild(2).gameObject.SetActive(true);
        }
        else
        {
            string playerName = GameObject.FindGameObjectWithTag("Player").name;
            if (playerName == "Soul")
            {
                Vector2 localDirection = transform.InverseTransformVector(Camera.main.transform.position - transform.position);
                transform.GetChild(0).localPosition = offset + localDirection.normalized * 0.3f;
            }
            else transform.GetChild(0).localPosition = offset + lookat;
        }
    }
    void WhileBlink()
    {
        time += Time.deltaTime;
        if (time > 0.1f)
        {
            lookat = Random.insideUnitCircle.normalized * 0.3f;
            transform.GetChild(2).gameObject.SetActive(false);
            update = WhileActive;
        }
    }
}
