using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : Creature
{
    float speed = 4.0f;
    public float vertical = 0.0f;
    public float horizontal = 0.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    void Start()
    {
        SetState("Hollow");
        GetComponent<MeshRenderer>().material.color = new Color32(75, 75, 75, 255);
    }

    void Movement()
    {
        float horizontalGoal = 0.0f;
        if (isActive)
        {
            Camera.main.GetComponent<CameraMovement>().lookat = transform.position;
            if (Input.GetKey(KeyCode.D)) { horizontalGoal += speed; }
            if (Input.GetKey(KeyCode.A)) { horizontalGoal -= speed; }
        }
        horizontal = Mathf.Lerp(horizontal, horizontalGoal, (acceleration * Time.fixedDeltaTime) / Mathf.Abs(horizontal - horizontalGoal));
        GameObject floor = CollidesWith("Floor");
        if (floor != null)
        {
            if (Input.GetKey(KeyCode.Space) && isActive) { vertical = 9.81f; }
            else if (!Physics2D.GetIgnoreCollision(GetComponent<Collider2D>(), floor.GetComponent<Collider2D>())) { vertical = Mathf.Max(0.0f, vertical); }
        }
        else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); }
        transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E) && cameraWeight == 0.0f) //Can't leave when seen
        {
            SetState("Hollow");
            Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position, Quaternion.identity);
        }
        if (Input.GetKey(KeyCode.Escape))
            Game.ActivateMenu("GameMenu");
    }

    void Arise()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
            SetState("Default");
    }

    public override void SetState(string stateName)
    {
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Arise": tag = "Player"; isActive = false; updates.Add(Arise); timer = 0.0f; break;
            case "Hollow": tag = "Hollow"; isActive = false; fixedUpdates.Add(Movement); break;
            default: isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); break;
        }
    }

    public override void IsVisible(bool visible)
    {
        if (isActive)
        {
            if (visible) { cameraWeight = Mathf.Min(cameraWeight + Time.deltaTime, 1.0f); }
            else { cameraWeight = Mathf.Max(cameraWeight - Time.deltaTime, 0.0f); }
            Camera.main.orthographicSize = Mathf.Lerp(5, 4, cameraWeight);
            Camera.main.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, (byte)Mathf.Lerp(0, 255, cameraWeight));
            if (cameraWeight == 1.0f && !Game.MenuActive("DeathMenu"))
            {
                isActive = false;
                Game.ActivateMenu("DeathMenu");
            }
        }
    }
}