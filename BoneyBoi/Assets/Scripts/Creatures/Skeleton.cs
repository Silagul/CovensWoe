using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Skeleton : Creature
{
    public Animator anim;
    public float currentSpeed;
    public float speed = 4.0f;
    public float vertical = 0.0f;
    public float horizontal = 0.0f;
    float acceleration = 16.0f;
    float timer = 0.0f;
    public bool canRotate = true;

    private GameManager gameManager;
    private Vector3 childPosition;

    public AudioClip[] movementAudioArray;
    public AudioClip[] collapseAudio;
    public AudioClip buildAudio;
    public AudioClip landingAudio;

    public PolygonCollider2D defaultCollider;
    public PolygonCollider2D hollowCollider;

    void Start()
    {
        creatures.Add(this);
        collisions.Add("Floor", new List<GameObject>());
        collisions.Add("Movable", new List<GameObject>());
        collisions.Add("Slowdown", new List<GameObject>());
        anim = GetComponent<Animator>();
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        transform.localScale = new Vector3(0.15f, 0.15f, 1);
        currentSpeed = speed;
        SetState("Hollow");
    }

    private void UpdateChildPosition()
    {
        childPosition = GameObject.Find("Human").transform.position;
        Invoke("UpdateChildPosition", 10f);
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
                    AudioManager.CreateAudio(movementAudioArray[Random.Range(0, movementAudioArray.Length)], false, true, transform);
            }
            if (Input.GetKey(InputManager.instance.left))
            {
                horizontalGoal -= currentSpeed;
                if (floor != null)
                    AudioManager.CreateAudio(movementAudioArray[Random.Range(0, movementAudioArray.Length)], false, true, transform);
            }
        }
        horizontal = Mathf.Lerp(horizontal, horizontalGoal, (acceleration * Time.fixedDeltaTime) / Mathf.Abs(horizontal - horizontalGoal));

        if (floor != null)
        {
            if (Input.GetKey(InputManager.instance.jump) && isActive)
            {
                vertical = Mathf.Sqrt(-2.0f * -9.81f * 4.4f);
                SetState("Jump");
            }
            else
                vertical = Mathf.Max(0.0f, vertical);
        }
        else vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime);
        transform.position += new Vector3(horizontal, vertical) * Time.fixedDeltaTime;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        if (canRotate)
        {
            if (horizontal > 0.0f) transform.localScale = new Vector3(-0.15f, 0.15f, 1);
            else if (horizontal < 0.0f) transform.localScale = new Vector3(0.15f, 0.15f, 1);
        }
        if (transform.localScale.x > 0) anim.SetFloat("Horizontal", -horizontal);
        else anim.SetFloat("Horizontal", horizontal);

        Movable movable;
        if (floor != null && (movable = CollidesWith("Movable", "Box")?.GetComponent<Movable>()) != null)
            movable.Interact(this);
        if (anim.GetBool("Grappling"))
            if (!Movable.IsHolding()) anim.SetBool("Grappling", false);
        //ClampMovement();
    }

    void Interact()
    {
        if (Input.GetKeyDown(InputManager.instance.possess) && isActive)
        {
            SetState("Hollow");
            ReleasePossession();
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
    
    void ReleasePossession()
    {
        defaultCollider.enabled = false;
        hollowCollider.enabled = true;
        gameManager.TimeAsSkeleton();
        AudioManager.CreateAudio(collapseAudio[Random.Range(0, collapseAudio.Length)], false, true, transform);
        Instantiate(Resources.Load<GameObject>("Prefabs/Soul"), transform.position + Vector3.up, Quaternion.identity);
    }

    void Arise()
    {
        timer += Time.deltaTime;
        if (timer > 1.0f)
        {
            SetState("Default");
            hollowCollider.enabled = false;
            defaultCollider.enabled = true;
            gameManager.TimeSinceSkeleton();
            AudioManager.CreateAudio(buildAudio, false, true, transform);
            //childPosition = GameObject.Find("Human").transform.localPosition;
            UpdateChildPosition();
        }
    }

    void Jump()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
            SetState("Default");
    }


    public override void SetState(string stateName)
    {
        state = stateName;
        updates.Clear();
        fixedUpdates.Clear();
        switch (stateName)
        {
            case "Jump":
                isActive = false;
                anim.Play("Jumping");
                anim.SetBool("Foothold", false);
                updates.Add(Jump);
                timer = 0.0f;
                break;
            case "Arise":
                isActive = false;
                anim.SetBool("IsPossessed", true);
                tag = "Player";
                defaultCollider.tag = tag;
                hollowCollider.tag = tag;
                updates.Add(Arise);
                timer = 0.0f;
                break;
            case "Hollow":
                isActive = false;
                tag = "Hollow";
                defaultCollider.tag = tag;
                hollowCollider.tag = tag;
                anim.SetBool("IsPossessed", false);
                fixedUpdates.Add(Movement);
                Prompt.interactKey.SetActive(false);
                //CameraMovement.SetCameraMask(new string[] { "Default", "IgnoreRaycast", "Creature", "Player", "Physics2D" });
                break;
            case "Dead":
                isActive = false;
                tag = "Corpse";
                defaultCollider.tag = tag;
                hollowCollider.tag = tag;
                anim.SetBool("IsPossessed", false);
                ReleasePossession();
                break;
            case "Default":
                tag = "Player";
                defaultCollider.tag = tag;
                hollowCollider.tag = tag;
                isActive = true;
                fixedUpdates.Add(Movement);
                updates.Add(Interact);
                updates.Add(ClampMovement);
                //CameraMovement.SetCameraMask(new string[] { "Default", "IgnoreRaycast", "Creature", "Player", "Physics2D", "Unseen", "Object" });
                break;
        }
    }

    void OnDestroy()
    {
        creatures.Remove(this);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Floor" && !anim.GetBool("Foothold"))
        {
            anim.SetBool("Foothold", true);
            AudioManager.CreateAudio(landingAudio, false, true, transform);
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
}