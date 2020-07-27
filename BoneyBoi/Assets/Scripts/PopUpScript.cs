using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScript : MonoBehaviour
{
    public enum messageEnum
    {
        Movement,
        Possess,
        Interact
    };

    public messageEnum message;
    private GameObject canvasPopUp;
    private TextMeshProUGUI messageText;


    private void Start()
    {
        canvasPopUp = GameObject.Find("CanvasPopUp");
        messageText = canvasPopUp.GetComponentInChildren<TextMeshProUGUI>();
        canvasPopUp.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvasPopUp.SetActive(true);
            switch(message)
            {
                case messageEnum.Movement:
                    messageText.text = "Press " + InputManager.instance.left +  " & " + InputManager.instance.right
                        + " to move around & " + InputManager.instance.jump + " to jump.";
                    break;

                case messageEnum.Possess:
                    messageText.text = "Press " + InputManager.instance.interact + " to release your Soul. As Soul you can move around with "
                        + InputManager.instance.up + InputManager.instance.left + InputManager.instance.down + InputManager.instance.right
                        + " & " + InputManager.instance.interact + " to Possess a body";
                    break;

                case messageEnum.Interact:
                    messageText.text = "Press " + InputManager.instance.grab + " to Interact with levers & hold to move boxes";
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canvasPopUp.SetActive(false);
        }
    }
}
