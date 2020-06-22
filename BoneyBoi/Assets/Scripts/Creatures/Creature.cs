﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

public class Creature : MonoBehaviour
{
    public List<GameObject> collisions = new List<GameObject>();
    public BaseAI AI; //Used to control... creatures? Switchable.
    public bool isActive = true; //Controls whetheter active or not -> Sent to AI for use.
    public virtual void Activate() { isActive = true; } //IF special conditions are needed.
    public virtual void Deactivate() { isActive = false; }
    public virtual bool Possess() { return false; } //Attempts to possess
    public virtual void Retreat() { tag = "Hollow"; Deactivate(); } //"Hollow" to leave behind available for possession.

    void Update()
    {
        AI?.Action(this); //Whatever AI (or Player) decides to do
    }

    void FixedUpdate()
    {
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        AI?.Rotation(this); //Creature Rotation
        AI?.Movement(this); //Creature Movement
    }

    //Collect and Discard bunch of collision data
    void OnCollisionEnter(Collision collision)
    {
        collisions.Add(collision.gameObject);
    }

    void OnCollisionExit(Collision collision)
    {
        collisions.Remove(collision.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        collisions.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        collisions.Remove(other.gameObject);
    }

    //Whether object collides with certain tag and returns it
    public GameObject CollidesWith(string tag)
    {
        foreach (GameObject go in collisions)
            if (go != null && go.tag == tag)
                return go;
        return null;
=======
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
            if (go != null && go.tag == tag)
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

    public static float visibleTime = 0.0f;
    public void IsVisible(bool visible)
    {
        if (visible) { visibleTime = Mathf.Min(1.0f, visibleTime + (Time.deltaTime / Time.timeScale)); CameraMovement.darken = true; }
        else { visibleTime = Mathf.Max(0.0f, visibleTime - Time.deltaTime); }
        if (isActive)
        {
            if (visibleTime == 1.0f)
            {
                if (Game.menu == null || !Game.MenuActive("DeathMenu"))
                {
                    isActive = false;
                    Game.ActivateMenu("DeathMenu");
                }
            }
        }
>>>>>>> ac8b6a67c187daa1efb2917ab2a88b894832f905
    }
}