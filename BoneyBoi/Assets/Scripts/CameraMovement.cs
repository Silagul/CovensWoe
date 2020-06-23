using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 lookat = Vector2.zero;
    public static bool willDie = false;
    void Start()
    {
        transform.GetChild(0).localScale = new Vector3(Screen.width / 64.0f, Screen.height / 64.0f, 1);
    }

    void Update()
    {
        if (Game.menu != null) darken = true;
        else darken = false;
        GameObject player;
        if ((player = GameObject.FindGameObjectWithTag("Player")) != null)
            player.GetComponent<Creature>().IsVisible(); //Might overwrite darken value
        transform.position = (Vector3)lookat - new Vector3(0, 0, 20);
        Darken();
    }

    public static bool darken; //Darken camera view when active (used from creature class)
    float cameraWeight = 0.0f;
    public void Darken()
    {
        if (darken) { cameraWeight = Mathf.Min(cameraWeight + Time.deltaTime, 1.0f); }
        else { cameraWeight = Mathf.Max(cameraWeight - Time.deltaTime, 0.0f); }
        Camera.main.orthographicSize = Mathf.Lerp(5, 4, cameraWeight);
        Camera.main.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, (byte)Mathf.Lerp(0, 255, cameraWeight));
        Time.timeScale = Mathf.Lerp(1.0f, 0.1f, cameraWeight);
        Time.fixedDeltaTime = Mathf.Lerp(0.016667f, 0.001667f, cameraWeight);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
    }

    public static void SetCameraMask(string[] layers)
    {
        int layerIndex = 0;
        foreach (string layer in layers)
            layerIndex += 1 << LayerMask.NameToLayer(layer);
        Camera.main.cullingMask = layerIndex;
    }
}