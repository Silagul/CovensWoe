using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderScript : MonoBehaviour
{
    bool isDragging = false;
    void Start()
    {
        switch (name)
        {
            case "Volume": transform.localPosition = new Vector3(Options.optionsData.volume * 4.0f - 2.0f, 1.0f, 0); break;
            case "Brightness": transform.localPosition = new Vector3(Options.optionsData.brightness * 4.0f - 2.0f, 0.0f, 0); break;
        }
    }

    void Update()
    {
        if (isDragging)
        {
            float localX = transform.parent.InverseTransformPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)).x;
            localX = Mathf.Min(Mathf.Max(localX, -2.0f), 2.0f);
            transform.localPosition = new Vector3(localX, transform.localPosition.y, transform.localPosition.z);
            float weight = (localX + 2.0f) / 4.0f;
            if (!Input.GetKey(KeyCode.Mouse0))
            {
                switch (name)
                {
                    case "Volume": AudioListener.volume = weight; Options.optionsData.volume = weight; break;
                    case "Brightness": RenderSettings.ambientLight = Color32.Lerp(Color.black, Color.white, weight); Options.optionsData.brightness = weight; break;
                }
                isDragging = false;
            }
            else
            {
                switch (name)
                {
                    case "Volume": AudioListener.volume = weight; break;
                    case "Brightness": RenderSettings.ambientLight = Color32.Lerp(Color.black, Color.white, weight); break;
                }
            }
        }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            isDragging = true;
    }
}