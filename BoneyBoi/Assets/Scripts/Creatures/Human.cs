using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Human : Creature
{
    Animator anim;
    float speed = 4.0f;
    public float vertical = 0.0f;
    public float horizontal = 0.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    void Start()
    {        anim = GetComponent<Animator>();
        SetState("Default");
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
            if (Input.GetKey(KeyCode.Space) && isActive) { vertical = 7.0f; anim.SetBool("Foothold", false); }
            else if (!Physics2D.GetIgnoreCollision(GetComponent<Collider2D>(), floor.GetComponent<Collider2D>()))
            {
                anim.SetBool("Foothold", true);
                vertical = Mathf.Max(0.0f, vertical);
            }
        }
        else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); anim.SetBool("Foothold", false); }
        transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        anim.SetFloat("Horizontal", Mathf.Abs(horizontal));
        if (horizontal > 0.0f) transform.localScale = new Vector3(-0.2f, 0.2f, 1);
        else if (horizontal < 0.0f) transform.localScale = new Vector3(0.2f, 0.2f, 1);
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
        state = stateName;
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Arise": anim.SetBool("IsPossessed", true); tag = "Player"; isActive = false; updates.Add(Arise); timer = 0.0f; break;
            case "Dead": tag = "Untagged"; isActive = false; break;
            case "Hollow": anim.SetBool("IsPossessed", false); tag = "Hollow"; isActive = false; fixedUpdates.Add(Movement); break;
            default: anim.SetBool("IsPossessed", true); tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); break;
        }
    }
}