using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skeleton : Creature
{
    public Animator anim;
    float speed = 4.0f;
    public float vertical = 0.0f;
    public float horizontal = 0.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    float duration = 0.0f;

    private GameManager gameManager;
    private Vector3 childPosition;
    private float distanceX = 15f;
    private float distanceY = 5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        transform.localScale = new Vector3(0.15f, 0.15f, 1);
        SetState("Hollow");
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
            if (Input.GetKey(KeyCode.Space) && isActive && !Input.GetKey(KeyCode.Q)) { vertical = Mathf.Sqrt(-2.0f * -9.81f * 4.2f); SetState("Jump"); }
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
        if (horizontal > 0.0f) transform.localScale = new Vector3(-0.15f, 0.15f, 1);
        else if (horizontal < 0.0f) transform.localScale = new Vector3(0.15f, 0.15f, 1);
    }

    void Interact()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SetState("Hollow");
            gameManager.TimeAsSkeleton();
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
            gameManager.TimeSinceSkeleton();
            childPosition = GameObject.Find("Human").transform.localPosition;
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
            case "Drag": break;
            case "Jump": anim.Play("Jumping"); isActive = false; updates.Add(Jump); timer = 0.0f; break;
            case "Arise": tag = "Player"; isActive = false; anim.SetBool("IsPossessed", true); updates.Add(Arise); timer = 0.0f; break;
            case "Hollow": tag = "Hollow"; anim.SetBool("IsPossessed", false); isActive = false; fixedUpdates.Add(Movement);
                CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D" }); break;
            case "Dead": tag = "Corpse"; isActive = false; SetState("Hollow");
                Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position + Vector3.up, Quaternion.identity); break;
            default: tag = "Player"; isActive = true; fixedUpdates.Add(Movement); updates.Add(Interact); updates.Add(ClampMovement);
                CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D", "Unseen" }); break;
        }
    }
}