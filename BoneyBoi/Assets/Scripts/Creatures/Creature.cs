using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }
}