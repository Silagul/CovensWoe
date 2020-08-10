using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    public static EndGame instance;
    System.Action update;
    float time = 0.0f;
    float duration = 30.0f;
    Transform cameraTransform;

    void Start()
    {
        instance = this;
        update = Deactive;
        cameraTransform = Camera.main.transform;
    }

    void Update()
    {
        update.Invoke();
    }

    void Active()
    {
        time += Time.deltaTime;
        transform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y + Mathf.Lerp(-25.0f, 25.0f, time / duration), -5.0f);
        if (time >= duration)
        {
            GameManager.instance.SetGameActive(false);
            GameObject mainMenu = GameManager.instance.GetMenu("MainMenu");
            mainMenu.SetActive(true);
            mainMenu.transform.parent.Find("BackgroundImage").gameObject.SetActive(true);
        }
    }
    
    void Deactive()
    {

    }

    public void SetActive()
    {
        update = Active;

    }
}