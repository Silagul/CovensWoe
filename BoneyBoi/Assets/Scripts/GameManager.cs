using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private int score = 0;
    private Scene thisScene;

    private void Awake()
    {
        thisScene = SceneManager.GetActiveScene();
        AnalyticsEvent.LevelStart(thisScene.name, thisScene.buildIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //Dictionary<string, object> customParams = new Dictionary<string, object>();
            //customParams.Add("Score", score);
            AnalyticsEvent.Custom("Score", new Dictionary<string, object>
            {
                {"score", score}
            });
            AnalyticsResult ar = AnalyticsEvent.Custom("Score");
            Debug.Log("Result is " + ar.ToString());
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            AddScore();
        }

    }

    public void AddScore()
    {
        score++;
        Debug.Log(score);
    }




    public Vector3 GetVectorFromAngle(float angle)  //this is used to get vector3 form angle
    {
        float angleRad = angle * (Mathf.PI / 180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }

    public float GetAngleFromVectorFloat(Vector3 dir)    //this is used to get angle from vector3
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }
}
