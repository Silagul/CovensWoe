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

    private Vector3 childPosition;
    private float distanceX = 15f;
    private float distanceY = 5f;


    private GameManager gameManager;

    public AudioClip flyingAudio;
    public AudioClip possessInAudio;
    public AudioClip possessOutAudio;

    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        distanceX = gameManager.soulDistanceX;
        distanceY = gameManager.soulDistanceY;
        name = name.Substring(0, name.Length - 7);
        SetState("Default");
        GetComponent<MeshRenderer>().material.color = new Color32(255, 255, 255, 255);
        transform.parent = GameManager.world.transform;
        gameManager.TimeSinceSoul();
        childPosition = GameObject.Find("Human").transform.localPosition;
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
        if (Input.GetKey(KeyCode.E) && timer > 1.0f)
        {
            GameObject target;
            if ((target = CollidesWith("Hollow")) != null)
                if (target.GetComponent<Creature>().Possess())
                {
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
        GameObject go = Instantiate(Resources.Load<GameObject>("Prefabs/Ectoplasm"), transform.position + Random.insideUnitSphere * 0.5f, Quaternion.identity);
        go.transform.parent = transform.parent;
    }

    void Vanish()
    {
        timer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer);
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