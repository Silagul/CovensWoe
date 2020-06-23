using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Soul : Creature
{
    Vector2 movement = Vector2.zero;
    float speed = 4.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    void Start()
    {
        SetState("Default");
        GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255);
    }

    void Interact()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(KeyCode.E) && timer > 1.0f)
        {
            GameObject target;
            if ((target = CollidesWith("Hollow")) != null)
                if (target.GetComponent<Creature>().Possess())
                    SetState("Possession");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Game.menu == null)
                Game.ActivateMenu("GameMenu");
            else if (Game.MenuActive("GameMenu"))
                Destroy(Game.menu);
            else if (Game.MenuActive("OptionsMenu"))
            {
                Options.SaveData();
                Game.ActivateMenu("GameMenu");
            }
        }
    }

    void Movement()
    {
        Vector2 movementGoal = Vector2.zero;
        if (isActive)
        {
            Camera.main.GetComponent<CameraMovement>().lookat = transform.position;
            if (Input.GetKey(KeyCode.W)) { movementGoal.y += 1.0f; }
            if (Input.GetKey(KeyCode.D)) { movementGoal.x += 1.0f; }
            if (Input.GetKey(KeyCode.S)) { movementGoal.y -= 1.0f; }
            if (Input.GetKey(KeyCode.A)) { movementGoal.x -= 1.0f; }
            movementGoal = movementGoal.normalized * speed;
        }
        movement = Vector2.Lerp(movement, movementGoal,
            (acceleration * Time.fixedDeltaTime) / Vector2.Distance(movement, movementGoal));
        GetComponent<Rigidbody2D>().velocity = movement;
        Instantiate(Resources.Load<GameObject>("Prefabs/Ectoplasm"), transform.position + Random.insideUnitSphere * 0.5f, Quaternion.identity);
    }

    void Vanish()
    {
        timer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer);
        if (timer > 1.0f)
            Destroy(gameObject);
    }

    public override void SetState(string stateName)
    {
        state = stateName;
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Possession": isActive = false; fixedUpdates.Add(Movement); updates.Add(Vanish); timer = 0.0f; break;
            case "Dead": SetState("default"); break;
            default: tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); timer = 0.0f; break;
        }
    }
}