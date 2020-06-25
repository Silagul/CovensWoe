using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ColorShroom : Interactable
{
    public static ColorShroom[] colorShrooms = new ColorShroom[5];
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
        colorShrooms[index] = this;
    }

    void Update()
    {
        if (time + duration < Time.time)
        {
            isActive = false;
            time = Mathf.Infinity;
            foreach (ColorShroom colorMatch in colorShrooms)
                colorMatch.color = new Color32(127, 127, 127, 255);
        }
    }

    void OnTriggerEnter2D(Collider2D collision) { if (collision.tag == "Player") Interact(collision.GetComponent<Creature>()); }
    void OnTriggerStay2D(Collider2D collision) { }

    public override void Interact(Creature creature)
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

    public bool CheckForColorMatch()
    {
        for (int i = 0; i < 5; i++)
            if (colorShrooms[i].color.CompareRGB(new Color32(127, 127, 127, 255)))
                return false;
        return true;
    }
}