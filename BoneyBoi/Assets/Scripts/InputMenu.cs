using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputMenu : MonoBehaviour
{
    Transform inputPanel;
    Event keyEvent;
    Text buttonText;
    KeyCode newKey;

    bool waitingForKey;

    // Start is called before the first frame update
    void Start()
    {
        inputPanel = transform.Find("InputMenu");
        inputPanel.gameObject.SetActive(false);
        waitingForKey = false;

        for (int i = 0; i < inputPanel.childCount; i++)
        {
            if (inputPanel.GetChild(i).name == "JumpKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.jump.ToString(); }
            if (inputPanel.GetChild(i).name == "UpKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.up.ToString(); }
            if (inputPanel.GetChild(i).name == "DownKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.down.ToString(); }
            if (inputPanel.GetChild(i).name == "LeftKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.left.ToString(); }
            if (inputPanel.GetChild(i).name == "RightKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.right.ToString(); }
            if (inputPanel.GetChild(i).name == "InteractKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.interact.ToString(); }
            if (inputPanel.GetChild(i).name == "MenuKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.menu.ToString(); }
            if (inputPanel.GetChild(i).name == "GrabKey")
            { inputPanel.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = InputManager.instance.grab.ToString(); }
        }
    }

    void OnGUI()
    {
        keyEvent = Event.current;

        if(keyEvent.isKey && waitingForKey)
        {
            newKey = keyEvent.keyCode;
            waitingForKey = false;
        }
    }

    public void StartAssignment(string keyName)
    {
        if (!waitingForKey)
        { 
            StartCoroutine(AssingKey(keyName));
        }
    }

    public void SendText(Text text)
    {
        buttonText = text;
    }

    IEnumerator WaitForKey()
    {
        while (!keyEvent.isKey)
            yield return null;
    }

    public IEnumerator AssingKey(string keyName)
    {
        waitingForKey = true;

        yield return WaitForKey();

        switch(keyName)
        {
            case "jump":
                InputManager.instance.jump = newKey;
                buttonText.text = InputManager.instance.jump.ToString();
                PlayerPrefs.SetString("JumpKey", InputManager.instance.jump.ToString());
                break;

            case "up":
                InputManager.instance.up = newKey;
                buttonText.text = InputManager.instance.up.ToString();
                PlayerPrefs.SetString("UpKey", InputManager.instance.up.ToString());
                break;

            case "down":
                InputManager.instance.down = newKey;
                buttonText.text = InputManager.instance.down.ToString();
                PlayerPrefs.SetString("DownKey", InputManager.instance.down.ToString());
                break;

            case "left":
                InputManager.instance.left = newKey;
                buttonText.text = InputManager.instance.left.ToString();
                PlayerPrefs.SetString("LeftKey", InputManager.instance.left.ToString());
                break;

            case "right":
                InputManager.instance.right = newKey;
                buttonText.text = InputManager.instance.right.ToString();
                PlayerPrefs.SetString("RightKey", InputManager.instance.right.ToString());
                break;

            case "interact":
                InputManager.instance.interact = newKey;
                buttonText.text = InputManager.instance.interact.ToString();
                PlayerPrefs.SetString("InteractKey", InputManager.instance.interact.ToString());
                break;

            case "menu":
                InputManager.instance.menu = newKey;
                buttonText.text = InputManager.instance.menu.ToString();
                PlayerPrefs.SetString("MenuKey", InputManager.instance.menu.ToString());
                break;

            case "grab":
                InputManager.instance.grab = newKey;
                buttonText.text = InputManager.instance.grab.ToString();
                PlayerPrefs.SetString("GrabKey", InputManager.instance.grab.ToString());
                break;
        }
    }

}
