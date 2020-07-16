using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;


    public KeyCode jump {get; set;}
    public KeyCode up {get; set;}
    public KeyCode down {get; set;}
    public KeyCode left {get; set;}
    public KeyCode right {get; set;}
    public KeyCode interact {get; set;}
    public KeyCode menu {get; set;}
    public KeyCode grab {get; set;}

    void Awake()
    {
        if (instance == null)
        {            
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(this);
        }

        jump = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("JumpKey", "Space"));
        up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("UpKey", "W"));
        down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("DownKey", "S"));
        left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LeftKey", "A"));
        right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("RightKey", "D"));
        interact = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("InteractKey", "E"));
        menu = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("MenuKey", "Escape"));
        grab = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("GrabKey", "Q"));
    }

    public void Update()
    {
        if (Input.GetKey(InputManager.instance.grab))
        {
            Debug.Log("Debug function, InputManager input");
        }
    }
}
