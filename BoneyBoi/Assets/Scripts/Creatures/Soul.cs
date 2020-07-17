﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Soul : Creature
{
    Vector2 movement = Vector2.zero;
    float speed = 4.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;

    Vector3 prevPosition;
    Vector3 nextPosition;
    private Vector3 childPosition;
    private float distanceX = 15f;
    private float distanceY = 5f;


    private GameManager gameManager;

    public AudioClip flyingAudio;
    public AudioClip possessInAudio;
    public AudioClip possessOutAudio;

    void Start()
    {
        collisions.Add("Hollow", new List<GameObject>());
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        distanceX = gameManager.soulDistanceX;
        distanceY = gameManager.soulDistanceY;
        name = name.Substring(0, name.Length - 7);
        SetState("Default");
        GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255);
        transform.parent = GameManager.world.transform;
        gameManager.TimeSinceSoul();
        childPosition = GameObject.Find("Human").transform.position;
        AudioManager.CreateAudio(possessOutAudio, false, true, this.transform);
        AudioManager.CreateAudio(flyingAudio, true, false, this.transform);
    }

    private void ClampMovement()
    {
        if (transform.position.x >= childPosition.x + distanceX)
        {
            transform.position = new Vector3(childPosition.x + distanceX, transform.position.y, 0f);
        }

        else if (transform.position.x <= childPosition.x - distanceX)
        {
            transform.position = new Vector3(childPosition.x - distanceX, transform.position.y, 0f);
        }

        if (transform.position.y >= childPosition.y + distanceY)
        {
            transform.position = new Vector3(transform.position.x, childPosition.y + distanceY, 0f);
        }

        else if (transform.position.y <= childPosition.y - distanceY)
        {
            transform.position = new Vector3(transform.position.x, childPosition.y - distanceY, 0f);
        }
    }

    void Interact()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(InputManager.instance.interact) && timer > 1.0f)
        {
            GameObject target;
            if ((target = CollidesWith("Hollow")) != null)
                if (target.GetComponent<Creature>().Possess())
                {
                    prevPosition = transform.position;
                    nextPosition = target.transform.position + Vector3.up;
                    AudioManager.CreateAudio(possessInAudio, false, true, this.transform);
                    SetState("Possession");
                }

        }
        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    if (GameManager.menu == null)
        //        GameManager.ActivateMenu("GameMenu");
        //    else if (GameManager.MenuActive("GameMenu"))
        //        Destroy(GameManager.menu);
        //    else if (GameManager.MenuActive("OptionsMenu"))
        //    {
        //        Options.SaveData();
        //        GameManager.ActivateMenu("GameMenu");
        //    }
        //}
    }

    void Movement()
    {
        Vector2 movementGoal = Vector2.zero;
        Camera.main.GetComponent<CameraMovement>().lookat = transform.position;
        if (isActive)
        {
            if (Input.GetKey(InputManager.instance.up)) { movementGoal.y += 1.0f; }
            if (Input.GetKey(InputManager.instance.right)) { movementGoal.x += 1.0f; }
            if (Input.GetKey(InputManager.instance.down)) { movementGoal.y -= 1.0f; }
            if (Input.GetKey(InputManager.instance.left)) { movementGoal.x -= 1.0f; }
            movementGoal = movementGoal.normalized * speed;
        }
        movement = Vector2.Lerp(movement, movementGoal,
            (acceleration * Time.fixedDeltaTime) / Vector2.Distance(movement, movementGoal));
        GetComponent<Rigidbody2D>().velocity = movement;
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Ectoplasm"), transform.position + Random.insideUnitSphere * 0.5f, Quaternion.identity);
        go.transform.parent = transform.parent;
    }

    void Vanish()
    {
        timer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer);
        transform.position = Vector3.Lerp(prevPosition, nextPosition, timer);
        if (timer > 1.0f)
        {
            gameManager.TimeAsSoul();
            Destroy(gameObject);
        }

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
            default: tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); updates.Add(ClampMovement); timer = 0.0f;
                CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D", "Object" }); break;
        }
    }
}