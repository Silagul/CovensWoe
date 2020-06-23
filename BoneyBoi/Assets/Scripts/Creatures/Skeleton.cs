using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skeleton : Creature
{
    Animator anim;
    float speed = 4.0f;
    public float vertical = 0.0f;
    public float horizontal = 0.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    void Start()
    {
        anim = GetComponent<Animator>();
        SetState("Hollow");
        GetComponent<SpriteRenderer>().color = new Color32(75, 75, 75, 255);
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
            if (Input.GetKey(KeyCode.Space) && isActive && !Input.GetKey(KeyCode.Q)) { vertical = 9.81f; anim.SetBool("Foothold", false); }
            else if (!Physics2D.GetIgnoreCollision(GetComponent<Collider2D>(), floor.GetComponent<Collider2D>()))
            {
                anim.SetBool("Foothold", true);
                vertical = Mathf.Max(0.0f, vertical);
            }
        }
        else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); }
        transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        anim.SetFloat("Horizontal", horizontal);
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
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
        state = stateName;
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Arise": tag = "Player"; isActive = false; updates.Add(Arise); timer = 0.0f; break;
            case "Hollow": tag = "Hollow"; isActive = false; fixedUpdates.Add(Movement);
                CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D" }); break;
            case "Dead": tag = "Corpse"; isActive = false; SetState("Hollow");
                Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position + Vector3.up, Quaternion.identity); break;
            default: tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact);
                CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D", "Unseen" }); break;
        }
    }
}