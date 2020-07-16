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
                    messageText.text = "Press A & D to move around & Space to jump.";
                    break;

                case messageEnum.Possess:
                    messageText.text = "Press E to Possess";
                    break;

                case messageEnum.Interact:
                    messageText.text = "Press Q to Interact with levers & hold to move boxes";
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
