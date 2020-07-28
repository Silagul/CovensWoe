using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpScript : MonoBehaviour
{
    public enum messageEnum
    {
        Box,
        Possess,
        Lever
    };

    public messageEnum message;
    private GameObject canvasPopUp;
    private TextMeshProUGUI messageText;
    private TextMeshProUGUI buttonText;


    private void Start()
    {
        canvasPopUp = GameObject.Find("CanvasPopUp");
        messageText = canvasPopUp.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        buttonText = canvasPopUp.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        canvasPopUp.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            canvasPopUp.SetActive(true);
            switch(message)
            {
                case messageEnum.Box:
                    messageText.text = "Hold";
                    buttonText.text = InputManager.instance.interact.ToString();
                    break;

                case messageEnum.Possess:
                    messageText.text = "Possess";
                    buttonText.text = InputManager.instance.possess.ToString();
                    break;

                case messageEnum.Lever:
                    messageText.text = "Interact";
                    buttonText.text = InputManager.instance.interact.ToString();
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
