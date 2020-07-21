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
    float duration = 0.0f;
    public bool canRotate = true;

    private GameManager gameManager;
    private Vector3 childPosition;
    private float distanceX = 25f;

    private bool hasLanded = false;

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
        distanceX = gameManager.soulDistanceX;
        transform.localScale = new Vector3(0.15f, 0.15f, 1);
        currentSpeed = speed;
        SetState("Hollow");
    }

    private void UpdateChildPosition()
    {
        childPosition = GameObject.Find("Human").transform.position;
        Invoke("UpdateChildPosition", 10f);
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
    }

    void Movement()
    {
        float horizontalGoal = 0.0f;
        GameObject floor = CollidesWith("Floor");
        if (CollidesWith("Slowdown") == null)
            currentSpeed = speed;
        else
            currentSpeed = speed * 0.5f;
        if (isActive)
        {
            Camera.main.GetComponent<CameraMovement>().lookat = transform.position + Vector3.up;
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

                if (floor != null)
                {
                    AudioManager.CreateAudio(movementAudioArray[Random.Range(0, movementAudioArray.Length)], false, true, this.transform);
                }
            }
        }
        horizontal = Mathf.Lerp(horizontal, horizontalGoal, (acceleration * Time.fixedDeltaTime) / Mathf.Abs(horizontal - horizontalGoal));

        if (floor != null)
        {
            if (Input.GetKey(InputManager.instance.jump) && isActive && !Input.GetKey(InputManager.instance.grab))
            {
                hasLanded = false;
                vertical = Mathf.Sqrt(-2.0f * -9.81f * 4.4f);
                SetState("Jump");
            }
            else if (!Physics2D.GetIgnoreCollision(GetComponent<Collider2D>(), floor.GetComponent<Collider2D>()))
            {
                anim.SetBool("Foothold", true);
                vertical = Mathf.Max(0.0f, vertical);
            }

            if (anim.GetCurrentAnimatorStateInfo(0).IsName("Land") && hasLanded == false)
            {
                hasLanded = true;
                AudioManager.CreateAudio(landingAudio, false, true, transform);
            }

        }
        else { vertical = Mathf.Max(-9.81f, vertical - 9.81f * Time.fixedDeltaTime); anim.SetBool("Foothold", false); }
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
        //ClampMovement();
    }

    void Interact()
    {
        if (Input.GetKeyDown(InputManager.instance.interact))
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
            defaultCollider.enabled = true;
            hollowCollider.enabled = false;
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
                updates.Add(Arise);
                timer = 0.0f;
                break;
            case "Hollow":
                isActive = false;
                tag = "Hollow";
                anim.SetBool("IsPossessed", false);
                fixedUpdates.Add(Movement);
                //CameraMovement.SetCameraMask(new string[] { "Default", "IgnoreRaycast", "Creature", "Player", "Physics2D" });
                break;
            case "Dead":
                isActive = false;
                tag = "Corpse";
                anim.SetBool("IsPossessed", false);
                ReleasePossession();
                break;
            default:
                tag = "Player";
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
}