﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
<<<<<<< HEAD

public class Human : Creature
{
    void Start()
    {
        AI = new HumanAI();
    }

    //Human AI can be possessed
    public override bool Possess()
    {
        if (!isActive)
        {
            Activate();
            tag = "Player";
            AI = new HumanAI();
            return true;
        }
        return false;
=======
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
        GetComponent<SpriteRenderer>().color = new Color32(75, 75, 75, 255);
        transform.parent = null;
    }

    void Movement()
    {
        float horizontalGoal = 0.0f;
        if (isActive)
        {
            Camera.main.GetComponent<CameraMovement>().lookat = transform.position + Vector3.up;
            if (Input.GetKey(KeyCode.D)) { horizontalGoal += speed; }
            if (Input.GetKey(KeyCode.A)) { horizontalGoal -= speed; }
        }
        horizontal = Mathf.Lerp(horizontal, horizontalGoal, (acceleration * Time.fixedDeltaTime) / Mathf.Abs(horizontal - horizontalGoal));
        GameObject floor = CollidesWith("Floor");
        if (floor != null)
        {
            if (Input.GetKey(KeyCode.Space) && isActive) { vertical = 7.0f; }
            else if (!Physics2D.GetIgnoreCollision(GetComponent<Collider2D>(), floor.GetComponent<Collider2D>())) { vertical = Mathf.Max(0.0f, vertical); }
        }
        else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); }
        transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void Interact()
    {
        if (isActive && Input.GetKeyDown(KeyCode.E) && visibleTime == 0.0f)
        {
            SetState("Hollow");
            Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position + Vector3.up, Quaternion.identity);
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
            default: tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); break;
        }
>>>>>>> ac8b6a67c187daa1efb2917ab2a88b894832f905
    }
}