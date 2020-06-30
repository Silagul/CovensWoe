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

    private GameManager gameManager;
    private float realStartTime = 0f;

    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        realStartTime = Time.timeSinceLevelLoad;
        gameManager.GetRealStartTime(realStartTime);

        transform.parent = GameManager.world.transform;
        name = name.Substring(0, name.Length - 7);
        anim = GetComponent<Animator>();
        SetState("Default");
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
            if (Input.GetKeyDown(KeyCode.Space) && isActive) { vertical = Mathf.Sqrt(-2.0f * -9.81f * 2.4f); SetState("Jump"); }
            else if (!Physics2D.GetIgnoreCollision(GetComponent<Collider2D>(), floor.GetComponent<Collider2D>()))
            {
                anim.SetBool("Foothold", true);
                vertical = Mathf.Max(0.0f, vertical);
            }
        }
        else { vertical = Mathf.Max(-53.0f, vertical - 9.81f * Time.deltaTime); anim.SetBool("Foothold", false); }
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
            gameManager.TimeAsChild();
            Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position + Vector3.up, Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.menu == null)
                GameManager.ActivateMenu("GameMenu");
            else if (GameManager.MenuActive("GameMenu"))
                Destroy(GameManager.menu);
            else if (GameManager.MenuActive("OptionsMenu"))
            {
                Options.SaveData();
                GameManager.ActivateMenu("GameMenu");
            }
        }
    }

    void Arise()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            SetState("Default");
            gameManager.TimeSinceChild();
        }

    }

    void Jump()
    {
        timer += Time.deltaTime;
        if (timer > 0.5f)
            SetState("Default");
    }

    public override void SetState(string stateName)
    {
        state = stateName;
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Jump": anim.Play("Jump"); isActive = false; updates.Add(Jump); timer = 0.0f; break;
            case "Arise": anim.SetBool("IsPossessed", true); tag = "Player"; isActive = false; updates.Add(Arise); timer = 0.0f; break;
            case "Dead": dying = true; anim.SetBool("IsPossessed", false); isActive = false; break;
            case "Hollow": anim.SetBool("IsPossessed", false); tag = "Hollow"; isActive = false; fixedUpdates.Add(Movement); break;
            default: anim.SetBool("IsPossessed", true); tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        collisions.Add(collision.gameObject);
        float t = vertical / -9.81f;
        float fallDistance = -9.81f * t * t * 0.5f;
        if (fallDistance < -6.0f)
        {
            anim.SetBool("Foothold", true);
            SetState("Dead");
        }
    }
}