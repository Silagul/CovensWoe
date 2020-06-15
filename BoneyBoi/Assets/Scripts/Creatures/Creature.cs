﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Creature : MonoBehaviour
{
    protected List<System.Action> updates = new List<System.Action>();
    protected List<System.Action> fixedUpdates = new List<System.Action>();

    List<GameObject> collisions = new List<GameObject>();
    void OnCollisionEnter2D(Collision2D collision) { collisions.Add(collision.gameObject); }
    void OnCollisionExit2D(Collision2D collision) { collisions.Remove(collision.gameObject); }
    void OnTriggerEnter2D(Collider2D collider) { collisions.Add(collider.gameObject); }
    void OnTriggerExit2D(Collider2D collider) { collisions.Remove(collider.gameObject); }
    public GameObject CollidesWith(string tag)
    {
        foreach (GameObject go in collisions)
            if (go.tag == tag)
                return go;
        return null;
    }
    public bool isActive;

    void Update()
    {
        for (int i = updates.Count - 1; i >= 0; i--)
            updates[i].Invoke();
    }
    void FixedUpdate()
    {
        for (int i = fixedUpdates.Count - 1; i >= 0; i--)
            fixedUpdates[i].Invoke();
    }

    public virtual void SetState(string stateName) { }
    public bool Possess()
    {
        if (tag == "Hollow")
        {
            SetState("Arise");
            return true;
        }
        return false;
    }

    protected float cameraWeight = 0.0f;
    public virtual void IsVisible(bool visible)
    {
        cameraWeight = Mathf.Max(cameraWeight - Time.deltaTime, 0.0f);
        Camera.main.orthographicSize = Mathf.Lerp(5, 4, cameraWeight);
        Camera.main.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, (byte)Mathf.Lerp(0, 255, cameraWeight));
    }
}