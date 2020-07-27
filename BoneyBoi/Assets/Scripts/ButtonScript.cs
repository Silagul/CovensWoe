using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    private GameManager gameManager;

    public AudioClip buttonPressAudio;
    public AudioClip buttonHighlightAudio;

    void Start()
    {
        gameManager = GameObject.Find("Game").GetComponent<GameManager>();
        buttonPressAudio = Resources.Load<AudioClip>("Audio/Menu/MenuButtonPress");

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
                    gameManager.SaveAnalytics();
                    //GameManager.ActivateMenu("MainMenu");
                    World.Remove();
                    Options.SaveData();
                    AudioManager.CreateAudio(buttonPressAudio, false, true, transform);
                    break;
                case "Continue":
                    GameManager.menu = null;
                    Destroy(transform.parent.gameObject);
                    AudioManager.CreateAudio(buttonPressAudio, false, true, transform);
                    break;
                case "Return":
                    AudioManager.CreateAudio(buttonPressAudio, false, true, transform);
                    if (GameObject.Find("World").transform.childCount != 0)
                    {
                        //GameManager.ActivateMenu("GameMenu");
                    }
                    else
                    {
                        //GameManager.ActivateMenu("MainMenu");
                    }
                    break;
                case "OptionsMenu": //GameManager.ActivateMenu("OptionsMenu"); AudioManager.CreateAudio(buttonPressAudio, false, true, transform); break;
                case "Start": //GameManager.ActivateMenu("ChapterMenu"); AudioManager.CreateAudio(buttonPressAudio, false, true, transform); break;
                case "Retry":
                    gameManager.SaveAnalytics();
                    World.Restart();
                    GameManager.menu = null;
                    Destroy(transform.parent.gameObject);
                    AudioManager.CreateAudio(buttonPressAudio, false, true, transform);
                    break;
                case "QuitGame":
                    AudioManager.CreateAudio(buttonPressAudio, false, true, transform);
                    #if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
                    #else
                              Application.Quit();
                    #endif 
                    break;
                default:
                    if (name.Substring(0, 5) == "Chunk")
                    {
                        Destroy(GameManager.menu);
                        Chunk.currentChunk = name;
                        World.Restart();
                    }
                    break;
            }
        }
    }
}