using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            switch (name)
            {
                case "MainMenu":
                    Game.ActivateMenu("MainMenu");
                    GameObject world;
                    if ((world = GameObject.Find("World(Clone)")) != null)
                        Destroy(world);
                    break;
                case "Continue":
                    Game.menu = null;
                    Destroy(transform.parent.gameObject);
                    break;
                case "Return":
                    if (transform.parent.name == "OptionsMenu(Clone)")
                        Options.SaveData();
                    if (GameObject.Find("World(Clone)") != null)
                        Game.ActivateMenu("GameMenu");
                    else
                        Game.ActivateMenu("MainMenu");
                    break;
                case "OptionsMenu":
                    Game.ActivateMenu("OptionsMenu");
                    break;
                case "Start":
                    Game.Restart();
                    Game.menu = null;
                    Destroy(transform.parent.gameObject);
                    break;
                case "Retry":
                    Game.Restart();
                    Game.menu = null;
                    Destroy(transform.parent.gameObject);
                    break;
                case "QuitGame":
                    Application.Quit();
                    break;
            }
        }
    }
}