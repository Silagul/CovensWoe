using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Creature : MonoBehaviour
{
    public bool isActive;
    protected List<System.Action> updates = new List<System.Action>();
    protected List<System.Action> fixedUpdates = new List<System.Action>();
    public static List<Creature> creatures = new List<Creature>();
    protected Dictionary<string, List<GameObject>> collisions = new Dictionary<string, List<GameObject>>();
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collisions.ContainsKey(collision.transform.tag))
            collisions[collision.transform.tag].Add(collision.gameObject);
    }
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collisions.ContainsKey(collision.transform.tag))
            collisions[collision.transform.tag].Remove(collision.gameObject);
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collisions.ContainsKey(collider.tag))
            collisions[collider.tag].Add(collider.gameObject);
    }
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collisions.ContainsKey(collider.tag))
            collisions[collider.tag].Remove(collider.gameObject);
    }

    public GameObject CollidesWith(string tag)
    {
        if (collisions.ContainsKey(tag))
            for (int i = 0; i < collisions[tag].Count; i++)
                if (collisions[tag][i] != null)
                    return collisions[tag][0];
        return null;
    }
    public GameObject CollidesWith(string tag, string name)
    {
        if (collisions.ContainsKey(tag))
            foreach (GameObject go in collisions[tag])
                if (go.name == name)
                    return go;
        return null;
    }
    public bool CollidesWith(string tag, GameObject gameObject)
    {
        if (collisions.ContainsKey(tag))
            foreach (GameObject go in collisions[tag])
                if (go != null && go == gameObject)
                    return true;
        return false;
    }
    public bool CollidesWithOtherThan(string tag, GameObject gameObject)
    {
        if (collisions.ContainsKey(tag))
            foreach (GameObject go in collisions[tag])
                if (go != null && go != gameObject)
                    return true;
        return false;
    }

    public static float minimumX;
    public static float maximumX;
    public static float minimumY;
    public static float maximumY;
    protected void ClampMovement()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        if (transform.position.x > maximumX) x = maximumX;
        else if (transform.position.x < minimumX) x = minimumX;
        if (transform.position.y > maximumY) y = maximumY;
        else if (transform.position.y < minimumY) y = minimumY;
        transform.position = new Vector3(x, y);
    }

    void Start()
    {
        collisions.Add("Floor", new List<GameObject>());
        collisions.Add("Hollow", new List<GameObject>());
        collisions.Add("Movable", new List<GameObject>());
    }

    void Update()
    {
        for (int i = updates.Count - 1; i >= 0; i--)
            updates[i].Invoke();
    }
    void FixedUpdate()
    {
        for (int i = fixedUpdates.Count - 1; i >= 0; i--)
            fixedUpdates[i].Invoke();
    }

    public string state;
    public virtual void SetState(string stateName) { }
    public bool Possess()
    {
        if (tag == "Hollow")
        {
            SetState("Arise");
            return true;
        }
        return false;
    }

    public static bool dying = false;
    public static float visibleTime = 0.0f;
    public void IsVisible()
    {
        GameManager gameManager;
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        if (dying/* || EnemySight.shouldDie*/) { visibleTime = Mathf.Min(1.0f, visibleTime + (Time.deltaTime / Time.timeScale)); CameraMovement.darken = true; }
        else { visibleTime = Mathf.Max(0.0f, visibleTime - Time.deltaTime); }
        if (visibleTime == 1.0f)
        {
            //if (GameManager.menu == null || !GameManager.MenuActive("DeathMenu"))
            if (gameManager.deathMenu.activeSelf == false)
            {
                gameManager.DeathCounter();
                isActive = false;
                //GameManager.ActivateMenu("DeathMenu");
                gameManager.DeathMenu();
            }
        }
    }
}