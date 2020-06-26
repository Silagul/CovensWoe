using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameManagerTest : MonoBehaviour
{
    //private float realStartTime = 0f;
    //[SerializeField]
    //private float timeAsChild = 0f;
    //private float timeSinceChild = 0f;
    //[SerializeField]
    //private float timeAsSkeleton = 0f;
    //private float timeSinceSkeleton = 0f;
    //[SerializeField]
    //private float timeAsSoul = 0f;
    //private float timeSinceSoul = 0f;
    //[SerializeField]
    //private int deaths = 0;

    //public void GetRealStartTime(float time)
    //{
    //    realStartTime = time;
    //}

    //public void TimeAsChild()
    //{
    //    timeAsChild = (Time.timeSinceLevelLoad - realStartTime) - timeSinceChild;
    //}

    //public void TimeSinceChild()
    //{
    //    timeSinceChild = (Time.timeSinceLevelLoad - realStartTime) - timeAsChild;
    //}

    //public void TimeAsSkeleton()
    //{
    //    timeAsSkeleton = (Time.timeSinceLevelLoad - realStartTime) - timeSinceSkeleton;
    //}

    //public void TimeSinceSkeleton()
    //{
    //    timeSinceSkeleton = (Time.timeSinceLevelLoad - realStartTime) - timeAsSkeleton;
    //}

    //public void TimeAsSoul()
    //{
    //    timeAsSoul = (Time.timeSinceLevelLoad - realStartTime) - timeSinceSoul;
    //}

    //public void TimeSinceSoul()
    //{
    //    timeSinceSoul = (Time.timeSinceLevelLoad - realStartTime) - timeAsSoul;
    //}

    //When logging time, take the current active states time too
    //for example if child is the active character run TimeAsChild()


    //private void OnApplicationQuit()
    //{
    //    GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
    //    switch (gameObject.name)
    //    {
    //        case "Human":
    //            {
    //                TimeAsChild();
    //                break;
    //            }

    //        case "Skeleton":
    //            {
    //                TimeAsSkeleton();
    //                break;
    //            }

    //        case "Soul":
    //            {
    //                TimeAsSoul();
    //                break;
    //            }

    //        default:
    //            break;
    //    }
        
    //    AnalyticsEvent.Custom("TimeSpentAs", new Dictionary<string, object>
    //        {
    //            {"Child", timeAsChild},
    //            {"Skeleton", timeAsSkeleton},
    //            {"Soul", timeAsSoul}
    //        });

    //    AnalyticsEvent.Custom("Deaths", new Dictionary<string, object>
    //    {
    //        {"Deaths", deaths}
    //    });

    //    AnalyticsResult ar = AnalyticsEvent.Custom("TimeSpentAs");
    //    Debug.Log("Result is " + ar.ToString());
    //}

    //public Vector3 GetVectorFromAngle(float angle)  //this is used to get vector3 form angle
    //{
    //    float angleRad = angle * (Mathf.PI / 180f);
    //    return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    //}

    //public float GetAngleFromVectorFloat(Vector3 dir)    //this is used to get angle from vector3
    //{
    //    dir = dir.normalized;
    //    float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    //    if (n < 0) n += 360;
    //    return n;
    //}
}
