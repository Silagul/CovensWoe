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
    private BoxCollider2D collider;


    private void Start()
    {
        canvasPopUp = GameObject.Find("CanvasPopUp");
        messageText = canvasPopUp.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>();
        buttonText = canvasPopUp.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>();
        //canvasPopUp.SetActive(false);
        collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if(transform.parent.tag == "Player" && collider.enabled == true)
        {
            collider.enabled = false;
        }

        else if (transform.parent.tag == "Hollow" && collider.enabled == false)
        {
            collider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch (message)
            {
                case messageEnum.Box:
                    if(collision.transform.parent.name == "Skeleton")
                    {
                        canvasPopUp.transform.GetChild(0).gameObject.SetActive(true);
                        canvasPopUp.transform.GetChild(1).gameObject.SetActive(true);
                        messageText.text = "Hold";
                        buttonText.text = InputManager.instance.grab.ToString();
                    }
                    break;

                case messageEnum.Possess:
                    canvasPopUp.transform.GetChild(0).gameObject.SetActive(true);
                    canvasPopUp.transform.GetChild(1).gameObject.SetActive(true);
                    messageText.text = "Possess";
                    buttonText.text = InputManager.instance.possess.ToString();
                    break;

                case messageEnum.Lever:
                    if (collision.transform.parent.name == "Skeleton" || collision.transform.parent.name == "Human")
                    {
                        canvasPopUp.transform.GetChild(0).gameObject.SetActive(true);
                        canvasPopUp.transform.GetChild(1).gameObject.SetActive(true);
                        messageText.text = "Interact";
                        buttonText.text = InputManager.instance.interact.ToString();
                    }
                    break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            //canvasPopUp.SetActive(false);
            canvasPopUp.transform.GetChild(0).gameObject.SetActive(false);
            canvasPopUp.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
}
