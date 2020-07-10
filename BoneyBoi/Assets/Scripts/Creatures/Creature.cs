using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Creature : MonoBehaviour
{
    protected List<System.Action> updates = new List<System.Action>();
    protected List<System.Action> fixedUpdates = new List<System.Action>();

    protected List<GameObject> collisions = new List<GameObject>();
    void OnCollisionEnter2D(Collision2D collision) { collisions.Add(collision.gameObject); }
    void OnCollisionExit2D(Collision2D collision) { collisions.Remove(collision.gameObject); }
    void OnTriggerEnter2D(Collider2D collider) { collisions.Add(collider.gameObject); }
    void OnTriggerExit2D(Collider2D collider) { collisions.Remove(collider.gameObject); }
    public GameObject CollidesWith(string tag)
    {
        foreach (GameObject go in collisions)
            if (go != null && go.tag == tag)
                return go;
        return null;
    }
    public bool isActive;

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
        if (dying/* || EnemySight.shouldDie*/) { visibleTime = Mathf.Min(1.0f, visibleTime + (Time.deltaTime / Time.timeScale)); CameraMovement.darken = true; }
        else { visibleTime = Mathf.Max(0.0f, visibleTime - Time.deltaTime); }
        if (visibleTime == 1.0f)
        {
            if (GameManager.menu == null || !GameManager.MenuActive("DeathMenu"))
            {
                GameManager gameManager;
                gameManager = GameObject.Find("Game").GetComponent<GameManager>();
                gameManager.DeathCounter();
                isActive = false;
                //GameManager.ActivateMenu("DeathMenu");
                gameManager.DeathMenu();
            }
        }
    }
}