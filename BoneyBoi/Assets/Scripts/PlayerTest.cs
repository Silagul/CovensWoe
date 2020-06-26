using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerTest : MonoBehaviour
{
    public string state = "";
    private float realStartTime = 0f;

    private GameManager gameManager;

    public void Wasted()
    {
        Debug.Log("Wasted");
    }

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        state = "Default";
        realStartTime = Time.timeSinceLevelLoad;
        gameManager.GetRealStartTime(realStartTime);
        //timeAsChild = 0f;
    }

    private void Update()
    {
        ///////////////////////////////////////////////FOR CHILD///////////////////////////////////////
        if(Input.GetKeyDown(KeyCode.D))
        {
            state = "Default";
            Debug.Log(state);
            gameManager.TimeSinceChild();
        }

        if (Input.GetKeyDown(KeyCode.H)) 
        {
            state = "Hollow";
            Debug.Log(state);
            gameManager.TimeAsChild();
        }

        //////////////////////////////////////////////FOR SKELETON/////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.S))
        {
            state = "Default";
            Debug.Log(state);
            gameManager.TimeSinceSkeleton();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = "Hollow";
            Debug.Log(state);
            gameManager.TimeAsSkeleton();
        }
    }
}
