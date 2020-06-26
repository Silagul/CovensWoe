using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    void Start()
    {
        if (name.Substring(0, 5) == "Chunk")
            if (!Options.optionsData.availableChunks.Contains(name))
            {
                GetComponent<SpriteRenderer>().color = new Color32(55, 55, 55, 255);
                name = $"Unavailable_{name}";
            }
    }

    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (name)
            {
                case "MainMenu":
                    Game.ActivateMenu("MainMenu");
                    World.Remove();
                    Options.SaveData();
                    break;
                case "Continue":
                    Game.menu = null;
                    Destroy(transform.parent.gameObject);
                    break;
                case "Return":
                    if (GameObject.Find("World").transform.childCount != 0) Game.ActivateMenu("GameMenu");
                    else Game.ActivateMenu("MainMenu");
                    break;
                case "OptionsMenu": Game.ActivateMenu("OptionsMenu"); break;
                case "Start": Game.ActivateMenu("ChapterMenu"); break;
                case "Retry":
                    World.Restart();
                    Game.menu = null;
                    Destroy(transform.parent.gameObject);
                    break;
                case "QuitGame":
                    Application.Quit();
                    break;
                default:
                    if (name.Substring(0, 5) == "Chunk") {
                        Destroy(Game.menu);
                        Chunk.currentChunk = name;
                        World.Restart();
                    }    
                    break;
            }
        }
    }
}