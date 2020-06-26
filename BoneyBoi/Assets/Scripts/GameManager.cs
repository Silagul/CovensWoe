using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public static GameObject menu;
    public static World world;

    private float realStartTime = 0f;
    [SerializeField]
    private float timeAsChild = 0f;
    private float timeSinceChild = 0f;
    [SerializeField]
    private float timeAsSkeleton = 0f;
    private float timeSinceSkeleton = 0f;
    [SerializeField]
    private float timeAsSoul = 0f;
    private float timeSinceSoul = 0f;
    [SerializeField]
    private int deaths = 0;

    void Start()
    {
        Options.Start();
        ActivateMenu("MainMenu");
        world = Instantiate(Resources.Load<GameObject>("Prefabs/World/World"), transform).GetComponent<World>();
    }

    public static void ActivateMenu(string menuName)
    {
        if (menu != null)
            Destroy(menu);
        menu = Instantiate(Resources.Load<GameObject>($"Prefabs/UI/{menuName}"), Camera.main.transform);
    }

    public static bool MenuActive(string menuName)
    {
        if (menu?.name == $"{menuName}(Clone)")
            return true;
        return false;
    }

    public void GetRealStartTime(float time)
    {
        realStartTime = time;
    }

    public void TimeAsChild()
    {
        timeAsChild = (Time.timeSinceLevelLoad - realStartTime) - timeSinceChild;
    }

    public void TimeSinceChild()
    {
        timeSinceChild = (Time.timeSinceLevelLoad - realStartTime) - timeAsChild;
    }

    public void TimeAsSkeleton()
    {
        timeAsSkeleton = (Time.timeSinceLevelLoad - realStartTime) - timeSinceSkeleton;
    }

    public void TimeSinceSkeleton()
    {
        timeSinceSkeleton = (Time.timeSinceLevelLoad - realStartTime) - timeAsSkeleton;
    }

    public void TimeAsSoul()
    {
        timeAsSoul = (Time.timeSinceLevelLoad - realStartTime) - timeSinceSoul;
    }

    public void TimeSinceSoul()
    {
        timeSinceSoul = (Time.timeSinceLevelLoad - realStartTime) - timeAsSoul;
    }

    public void DeathCounter()
    {
        deaths++;
    }

    private void OnApplicationQuit()    //This is for sending Analytics when quitting
    {
        SaveAnalytics();
        SendAnalytics();
    }

    public void SaveAnalytics()
    {
        if (GameObject.Find("Human") != null)
        {
            Debug.Log("Saved Analytics");
            GameObject gameObject = GameObject.FindGameObjectWithTag("Player");

            switch (gameObject.name)
            {
                case "Human":
                    {
                        TimeAsChild();
                        break;
                    }

                case "Skeleton":
                    {
                        TimeAsSkeleton();
                        break;
                    }

                case "Soul":
                    {
                        TimeAsSoul();
                        break;
                    }

                default:
                    break;
            }
        }
    }
    
    public void SendAnalytics()
    {
        AnalyticsEvent.Custom("TimeSpentAs", new Dictionary<string, object>
        {
            {"Child", timeAsChild},
            {"Skeleton", timeAsSkeleton},
            {"Soul", timeAsSoul}
        });

        AnalyticsEvent.Custom("Deaths", new Dictionary<string, object>
        {
            {"Deaths", deaths}
        });

        AnalyticsResult ar = AnalyticsEvent.Custom("TimeSpentAs");
        Debug.Log("TimeSpentAs result is " + ar.ToString());
        ar = AnalyticsEvent.Custom("Deaths");
        Debug.Log("Deaths result is " + ar.ToString());
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