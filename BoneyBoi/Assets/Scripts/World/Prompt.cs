using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prompt : MonoBehaviour
{
    public enum PromptKey { interact, possess }
    public PromptKey key;
    System.Action update;
    bool isActive;
    public static Prompt interactKey;
    public static Prompt possessKey;
    SpriteRenderer spriteRenderer;
    SpriteRenderer childRenderer;
    float alpha = 0.0f;

    void Start()
    {
        if (key == PromptKey.interact)
        {
            interactKey = this;
            Camera cam = Camera.main;
            transform.localPosition = new Vector3(cam.orthographicSize * cam.aspect, -cam.orthographicSize, 10.0f);
        }
        else
        {
            possessKey = this;
            Camera cam = Camera.main;
            transform.localPosition = new Vector3(cam.orthographicSize * cam.aspect, -cam.orthographicSize + 1.0f, 10.0f);
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        childRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.color = new Color32(255, 255, 255, 0);
        childRenderer.color = new Color32(255, 255, 255, 0);
        update = NoFade;
    }

    void Update()
    {
        update.Invoke();
    }

    void FadeIn()
    {
        alpha = Mathf.Min(alpha + Time.deltaTime, 1.0f);
        spriteRenderer.color = Color32.Lerp(new Color32(255, 255, 255, 0), new Color32(255, 255, 255, 255), alpha);
        childRenderer.color = spriteRenderer.color;
        if (alpha == 1.0f)
            update = NoFade;
    }

    void FadeOut()
    {
        alpha = Mathf.Max(alpha - Time.deltaTime, 0.0f);
        spriteRenderer.color = Color32.Lerp(new Color32(255, 255, 255, 0), new Color32(255, 255, 255, 255), alpha);
        childRenderer.color = spriteRenderer.color;
        if (alpha == 0.0f)
            update = NoFade;
    }

    void NoFade() { }

    public void SetActive(bool activate)
    {
        if (isActive != activate)
        {
            isActive = activate;
            if (activate) update = FadeIn;
            else update = FadeOut;
        }
    }
}
