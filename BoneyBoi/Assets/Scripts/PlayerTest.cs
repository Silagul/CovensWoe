using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class PlayerTest : MonoBehaviour
{
    public string state = "";
    //private float timeAsChild = 0f;
    //private float timeSinceChild = 0f;
    private float realStartTime = 0f;

    private GameManagerTest gameManagerTest;

    public void Wasted()
    {
        Debug.Log("Wasted");
    }

    private void Start()
    {
        gameManagerTest = GameObject.Find("GameManager").GetComponent<GameManagerTest>();
        state = "Default";
        realStartTime = Time.timeSinceLevelLoad;
        gameManagerTest.GetRealStartTime(realStartTime);
        //timeAsChild = 0f;
    }

    private void Update()
    {
        ///////////////////////////////////////////////FOR CHILD///////////////////////////////////////
        if(Input.GetKeyDown(KeyCode.D))
        {
            state = "Default";
            Debug.Log(state);
            gameManagerTest.TimeSinceChild();
        }

        if (Input.GetKeyDown(KeyCode.H)) 
        {
            state = "Hollow";
            Debug.Log(state);
            gameManagerTest.TimeAsChild();
        }

        //////////////////////////////////////////////FOR SKELETON/////////////////////////////////////
        if (Input.GetKeyDown(KeyCode.S))
        {
            state = "Default";
            Debug.Log(state);
            gameManagerTest.TimeSinceSkeleton();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            state = "Hollow";
            Debug.Log(state);
            gameManagerTest.TimeAsSkeleton();
        }

        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    Destroy(this.gameObject);
        //}
    }

    //private void OnDestroy()
    //{
    //    AnalyticsEvent.Custom("TimeSpentAsChild", new Dictionary<string, object>
    //        {
    //            {"childTime", timeAsChild}
    //        });
    //    AnalyticsResult ar = AnalyticsEvent.Custom("TimeSpentAsChild");
    //    Debug.Log("Result is " + ar.ToString());
    //}
}
