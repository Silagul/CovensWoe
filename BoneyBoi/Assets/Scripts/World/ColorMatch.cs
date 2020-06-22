using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorMatch : Interactable
{
    public static ColorMatch[] colorMatches = new ColorMatch[5];
    public Color32 color {
        get { return GetComponent<SpriteRenderer>().color; }
        set { GetComponent<SpriteRenderer>().color = value; } }
    public int index;
    public static float time = 0.0f;
    public static bool isActive = false;
    readonly float duration = 10.0f;

    void Start()
    {
        color = new Color32(127, 127, 127, 255);
        colorMatches[index] = this;
    }

    void Update()
    {
        if (time + duration < Time.time)
        {
            isActive = false;
            time = Mathf.Infinity;
            foreach (ColorMatch colorMatch in colorMatches)
                colorMatch.color = new Color32(127, 127, 127, 255);
        }
    }

    public override void Interact()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if (!isActive)
            {
                isActive = true;
                time = Time.time;
            }
            if (color.CompareRGB(new Color32(127, 127, 127, 255)))
                color = Random.ColorHSV();
            if (CheckForColorMatch())
                Debug.Log("Colors Match");
        }
    }

    public bool CheckForColorMatch()
    {
        for (int i = 0; i < 5; i++)
            if (colorMatches[i].color.CompareRGB(new Color32(127, 127, 127, 255)))
                return false;
        return true;
    }
}