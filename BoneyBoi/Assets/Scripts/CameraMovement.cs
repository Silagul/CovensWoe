using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Vector2 lookat = Vector2.zero;
    public float minX, minY, maxX, maxY;
    void Start()
    {
        transform.GetChild(0).localScale = new Vector3(Screen.width / 64.0f, Screen.height / 64.0f, 1);
    }

    void Update()
    {
        if (GameManager.menu != null) darken = true;
        else darken = false;
        GameObject player;
        if ((player = GameObject.FindGameObjectWithTag("Player")) != null)
            player?.GetComponent<Creature>().IsVisible(); //Might overwrite darken value
        float offsetX = Mathf.Max(-maxX, Mathf.Min(maxX, lookat.x - transform.position.x));
        float offsetY = Mathf.Max(-maxY, Mathf.Min(maxY, lookat.y - transform.position.y));
        float weightX = (Mathf.Abs(offsetX) - minX) / (maxX - Mathf.Abs(offsetX));
        float weightY = (Mathf.Abs(offsetY) - minY) / (maxY - Mathf.Abs(offsetY));
        if (offsetX > minX) offsetX -= Mathf.Lerp(0.0f, 4.0f, weightX) * Time.deltaTime;
        else if (offsetX < -minX) offsetX += Mathf.Lerp(0.0f, 4.0f, weightX) * Time.deltaTime;
        if (offsetY > minY) offsetY -= Mathf.Lerp(0.0f, 4.0f, weightY) * Time.deltaTime;
        else if (offsetY < -minY) offsetY += Mathf.Lerp(0.0f, 4.0f, weightY) * Time.deltaTime;
        transform.position = (Vector3)lookat - new Vector3(offsetX, offsetY, 20);
        Darken();
    }

    public static bool darken; //Darken camera view when active (used from creature class)
    float cameraWeight = 0.0f;
    public void Darken()
    {
        if (darken)
        {
            cameraWeight = Mathf.Min(cameraWeight + Time.deltaTime, 1.0f);
            Time.timeScale = Mathf.Lerp(1.0f, 0.1f, cameraWeight);
            Time.fixedDeltaTime = Mathf.Lerp(0.016667f, 0.001667f, cameraWeight);
        }
        else
        {
            cameraWeight = Mathf.Max(cameraWeight - Time.deltaTime, 0.0f);
        }
        Camera.main.orthographicSize = Mathf.Lerp(5, 4, cameraWeight);
        Camera.main.GetComponentInChildren<SpriteRenderer>().color = new Color32(0, 0, 0, (byte)Mathf.Lerp(0, 255, cameraWeight));
        Time.timeScale = Mathf.Lerp(1.0f, 0.1f, cameraWeight);
        Time.fixedDeltaTime = Mathf.Lerp(0.016667f, 0.001667f, cameraWeight);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player?.GetComponent<Creature>().IsVisible();
    }

    public static void SetCameraMask(string[] layers)
    {
        int layerIndex = 0;
        foreach (string layer in layers)
            layerIndex += 1 << LayerMask.NameToLayer(layer);
        Camera.main.cullingMask = layerIndex;
    }
}