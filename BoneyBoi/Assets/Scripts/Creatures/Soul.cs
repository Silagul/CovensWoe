using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Soul : Creature
{
    Vector2 movement = Vector2.zero;
    float speed = 8.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;

    Vector3 prevPosition;
    Transform nextVessel;

    private GameManager gameManager;

    public AudioClip flyingAudio;
    public AudioClip possessInAudio;
    public AudioClip possessOutAudio;
    public Collider2D defaultCollider;

    void Start()
    {
        collisions.Add("Hollow", new List<GameObject>());
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        name = name.Substring(0, name.Length - 7);
        SetState("Default");
        transform.parent = GameManager.world.transform;
        gameManager.TimeSinceSoul();
        AudioManager.CreateAudio(possessOutAudio, false, true, this.transform);
        AudioManager.CreateAudio(flyingAudio, true, false, this.transform);
    }

    void Interact()
    {
        timer += Time.deltaTime;
        if (Input.GetKey(InputManager.instance.possess) && timer > 1.0f)
        {
            GameObject target;
            if ((target = CollidesWith("Hollow")) != null)
            {
                Creature creature;
                if (target.transform.parent.TryGetComponent(out creature))
                {
                    if (creature.Possess())
                    {
                        prevPosition = transform.position;
                        nextVessel = target.transform;
                        AudioManager.CreateAudio(possessInAudio, false, true, transform);
                        SetState("Possession");
                    }
                }
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
    }

    void Vanish()
    {
        timer += Time.deltaTime;
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, timer);
        transform.position = Vector3.Lerp(prevPosition, nextVessel.position + Vector3.up, timer);
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
            case "Possession":
                isActive = false;
                updates.Add(Vanish);
                fixedUpdates.Add(Movement);
                timer = 0.0f;
                break;
            case "Dead":
                break;
            case "Default":
                isActive = true;
                tag = "Player";
                defaultCollider.tag = tag;
                updates.Add(Interact);
                updates.Add(ClampMovement);
                fixedUpdates.Add(Movement);
                timer = 0.0f;
                //CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D", "Object" });
                break;
        }
    }
}