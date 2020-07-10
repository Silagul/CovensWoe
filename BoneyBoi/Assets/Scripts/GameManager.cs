using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class GameManager : MonoBehaviour
{
    public static GameObject menu;
    public static World world;


    //These values are for Analytics
    private float realStartTime = 0f;
    //[SerializeField]
    private float currentTimeAsChild = 0f;
    private float timeAsChild = 0f;
    private float timeSinceChild = 0f;
    //[SerializeField]
    private float currentTimeAsSkeleton = 0f;
    private float timeAsSkeleton = 0f;
    private float timeSinceSkeleton = 0f;
    //[SerializeField]
    private float currentTimeAsSoul = 0f;
    private float timeAsSoul = 0f;
    private float timeSinceSoul = 0f;
    //[SerializeField]
    private int deaths = 0;
    public bool analyticsEnabled = true;

    //These determine how far the soul/skeleton can move from the player
    public float soulDistanceX;
    public float soulDistanceY;

    private bool gameActive = false;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject deathMenu;



    void Start()
    {
        Options.Start();
        //ActivateMenu("MainMenu");
        world = Instantiate(Resources.Load<GameObject>("Prefabs/World/World"), transform).GetComponent<World>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && gameActive == true)
        {
            if (pauseMenu.activeSelf == true)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.016667f;
            }

            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
            }
        }
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

    public void SaveOptions()
    {
        Options.SaveData();
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }

    public void SelectLevel(string chunk)
    {
        Chunk.currentChunk = chunk;
        World.Restart();
    }

    public void Continue()
    {
        SaveAnalytics();
        World.Restart();
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.016667f;
    }

    public void DeathMenu()
    {
        deathMenu.SetActive(true);
    }

    public void SetGameActive(bool isActive)
    {
        gameActive = isActive;
        if (gameActive == false)
        {
            World.Remove();
        }
    }

    public void GetRealStartTime(float time)    //This for getting the correct start time after main menu
    {
        realStartTime = time;
    }

    //These functions bellow are for calculating Analytics play times

    public void TimeAsChild()
    {
        currentTimeAsChild = (Time.timeSinceLevelLoad - realStartTime) - timeSinceChild;
    }

    public void TimeSinceChild()
    {
        timeSinceChild = (Time.timeSinceLevelLoad - realStartTime) - currentTimeAsChild;
    }

    public void TimeAsSkeleton()
    {
        currentTimeAsSkeleton = (Time.timeSinceLevelLoad - realStartTime) - timeSinceSkeleton;
    }

    public void TimeSinceSkeleton()
    {
        timeSinceSkeleton = (Time.timeSinceLevelLoad - realStartTime) - currentTimeAsSkeleton;
    }

    public void TimeAsSoul()
    {
        currentTimeAsSoul = (Time.timeSinceLevelLoad - realStartTime) - timeSinceSoul;
    }

    public void TimeSinceSoul()
    {
        timeSinceSoul = (Time.timeSinceLevelLoad - realStartTime) - currentTimeAsSoul;
    }

    public void DeathCounter()
    {
        deaths++;
        //Debug.Log(deaths);
    }

    private void OnApplicationQuit()    //This is for sending Analytics when quitting
    {
        SaveAnalytics();
        SendAnalytics();
    }

    public void SaveAnalytics() //This is for saving and resetting Analytics values
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

        timeAsChild += currentTimeAsChild;
        currentTimeAsChild = 0f;
        timeSinceChild = 0f;
        timeAsSkeleton += currentTimeAsSkeleton;
        currentTimeAsSkeleton = 0f;
        timeSinceSkeleton = 0f;
        timeAsSoul += currentTimeAsSoul;
        currentTimeAsSoul = 0f;
        timeSinceSoul = 0f;
    }
    
    public void SendAnalytics() //This funcition sends all of the Analytics data, call this when quitting the game
    {
        if (analyticsEnabled == true)
        {
            //Debug.Log("Child " + timeAsChild + " Skeleton " + timeAsSkeleton + " Soul " + timeAsSoul);
            Mathf.Round(timeAsChild);
            Mathf.Round(timeAsSkeleton);
            Mathf.Round(timeAsSoul);

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