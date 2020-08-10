using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class Human : Creature
{
    Animator anim;
    public float speed;
    public float currentSpeed;

    public float vertical = 0.0f;
    public float horizontal = 0.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    bool jumping = true;
    bool footheld = false;

    private GameManager gameManager;
    private float realStartTime = 0f;

    public AudioClip[] movementAudioArray;
    public AudioClip landingAudio;
    public AudioClip landingDeathAudio;
    public AudioClip deathAudio;

    //public PolygonCollider2D defaultCollider;
    public PolygonCollider2D defaultCollider;
    Vector2[] defaultPoints, hollowPoints, crouchPoints;
    public CapsuleCollider2D monsterCollider;

    void Start()
    {
        creatures.Add(this);
        defaultPoints = transform.Find("DefaultCollider").GetComponent<PolygonCollider2D>().points;
        hollowPoints = transform.Find("HollowCollider").GetComponent<PolygonCollider2D>().points;
        crouchPoints = transform.Find("CrouchCollider").GetComponent<PolygonCollider2D>().points;
        collisions.Add("Floor", new List<GameObject>());
        collisions.Add("Slowdown", new List<GameObject>());
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        realStartTime = Time.timeSinceLevelLoad;
        gameManager.GetRealStartTime(realStartTime);

        transform.parent = GameManager.world.transform;
        name = name.Substring(0, name.Length - 7);
        anim = GetComponent<Animator>();
        SetState("Default");
        defaultCollider.points = defaultPoints;
        currentSpeed = speed;
        Instantiate(Resources.Load<GameObject>("Prefabs/DeathBox"), GameManager.world.transform);
    }

    void Movement()
    {
        float horizontalGoal = 0.0f;
        GameObject floor = CollidesWith("Floor");
        if (CollidesWith("Slowdown") == null)
            currentSpeed = speed;
        else
            currentSpeed = speed * 0.5f;
        if (tag == "Player") Camera.main.GetComponent<CameraMovement>().lookat = transform.position + Vector3.up;
        if (isActive)
        {
            if (Input.GetKey(InputManager.instance.right))
            {
                horizontalGoal += currentSpeed;

                if (floor != null)
                {
                    AudioManager.CreateAudio(movementAudioArray[Random.Range(0, movementAudioArray.Length)], false, true, this.transform);
                }
            }

            if (Input.GetKey(InputManager.instance.left))
            {
                horizontalGoal -= currentSpeed;

                if(floor != null)
                {
                    AudioManager.CreateAudio(movementAudioArray[Random.Range(0, movementAudioArray.Length)], false, true, this.transform);
                }
            }

            if (Input.GetKey(InputManager.instance.crouch) && anim.GetFloat("Horizontal") == 0f)
            {
                defaultCollider.points = crouchPoints;
                monsterCollider.size = new Vector2(4f, 6f);
                monsterCollider.offset = new Vector2(0f, 3f);
                anim.SetBool("IsCrouching", true);
            }
            else
            {
                defaultCollider.points = defaultPoints;
                monsterCollider.size = new Vector2(4f, 12f);
                monsterCollider.offset = new Vector2(0f, 6f);
                anim.SetBool("IsCrouching", false);
            }
        }
        horizontal = Mathf.Lerp(horizontal, horizontalGoal, (acceleration * Time.fixedDeltaTime) / Mathf.Abs(horizontal - horizontalGoal));

        if (floor != null)
        {
            footheld = true;
            if (Input.GetKeyDown(InputManager.instance.jump) && isActive)
                SetState("Jump");
            else
                vertical = Mathf.Max(0.0f, vertical);
        }
        else
            vertical = Mathf.Max(-20.0f, vertical - 9.81f * Time.fixedDeltaTime);
        transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
        if (!jumping && footheld && !Input.GetKey(InputManager.instance.down))
        {
            footheld = false;
            RaycastHit2D hit = Physics2D.Raycast(transform.position + new Vector3(0, 0.1f), Vector2.down, 0.2f);
            if (hit) transform.position = hit.point;
        }
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (horizontal > 0.0f) transform.localScale = new Vector3(-0.2f, 0.2f, 1);
        else if (horizontal < 0.0f) transform.localScale = new Vector3(0.2f, 0.2f, 1);
        if (transform.localScale.x > 0) anim.SetFloat("Horizontal", -horizontal);
        else anim.SetFloat("Horizontal", horizontal);
    }

    void Interact()
    {
        if (isActive && Input.GetKeyDown(InputManager.instance.possess) && visibleTime == 0.0f)
        {
            SetState("Hollow");
            defaultCollider.points = hollowPoints;
            gameManager.TimeAsChild();
            Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position + Vector3.up, Quaternion.identity);
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

    void Arise()
    {
        timer += Time.deltaTime;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        if (timer > 1.0f)
        {
            SetState("Default");
            defaultCollider.points = defaultPoints;
            gameManager.TimeSinceChild();
        }
    }

    void Jump()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            jumping = true;
            SetState("Default");
        }
    }

    void Land()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        timer += Time.deltaTime;
        if (timer > 0.75f)
            SetState("Default");
    }

    public override void SetState(string stateName)
    {
        state = stateName;
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Land":
                isActive = false;
                updates.Add(Land);
                fixedUpdates.Add(Movement);
                timer = 0.0f;
                break;
            case "Jump":
                isActive = false;
                anim.Play("Jump");
                updates.Add(Jump);
                timer = 0.0f;
                vertical = Mathf.Sqrt(-2.0f * -9.81f * 2.4f);
                break;
            case "Arise":
                isActive = false;
                tag = "Player";
                defaultCollider.tag = tag;
                anim.SetBool("IsPossessed", true);
                updates.Add(Arise);
                timer = 0.0f;
                break;
            case "Dead":
                isActive = false;
                dying = true;
                anim.SetBool("IsPossessed", false);
                AudioManager.CreateAudio(deathAudio, false, false, transform);
                break;
            case "Hollow":
                isActive = false;
                tag = "Hollow";
                defaultCollider.tag = tag;
                anim.SetBool("IsPossessed", false);
                fixedUpdates.Add(Movement);
                break;
            case "Default":
                tag = "Player";
                defaultCollider.tag = tag;
                isActive = true;
                updates.Add(Interact);
                fixedUpdates.Add(Movement);
                anim.SetBool("IsPossessed", true);
                //CameraMovement.SetCameraMask(new string[] { "Default", "Creature", "Player", "Physics2D", "Object" });
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor" && !anim.GetBool("Foothold"))
        {
            jumping = false;
            vertical = Mathf.Max(0.0f, vertical);
            float t = vertical / -9.81f;
            float fallDistance = -9.81f * t * t * 0.5f;
            anim.SetBool("Foothold", true);
            AudioManager.CreateAudio(landingAudio, false, true, transform);
            if (fallDistance < -6.4f)
            {
                AudioManager.CreateAudio(landingDeathAudio, false, true, transform);
                SetState("Dead");
            }
        }
        if (collisions.ContainsKey(collision.transform.tag))
            collisions[collision.transform.tag].Add(collision.gameObject);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor" && !CollidesWithOtherThan("Floor", collision.gameObject))
            anim.SetBool("Foothold", false);
        if (collisions.ContainsKey(collision.transform.tag))
            collisions[collision.transform.tag].Remove(collision.gameObject);
    }

    public void Death()
    {
        SetState("Dead");
    }

    void OnDestroy()
    {
        creatures.Remove(this);
    }
}