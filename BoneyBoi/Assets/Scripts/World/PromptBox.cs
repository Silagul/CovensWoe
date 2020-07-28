using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PromptBox : MonoBehaviour
{
    public Prompt.PromptKey key;
    Prompt prompt;
    void Start()
    {
        if (key == Prompt.PromptKey.interact)
            prompt = Prompt.interactKey;
        else
            prompt = Prompt.possessKey;
    }
    
    void OnTriggerStay2D(Collider2D collision)
    {
        if (key == Prompt.PromptKey.interact)
            if (collision.tag == "Player")
                if (collision.transform.parent.name == "Skeleton")
                    prompt.SetActive(true);
                else { }
            else { }
        else if (collision.tag == "Player")
            prompt.SetActive(true);

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (key == Prompt.PromptKey.interact)
            if (collision.tag == "Player")
                if (collision.transform.parent.name == "Skeleton")
                    prompt.SetActive(false);
                else { }
            else { }
        else if (collision.tag == "Player")
            prompt.SetActive(false);
    }
}