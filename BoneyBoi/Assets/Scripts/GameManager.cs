using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.Audio;
using UnityEngine.UI;

#pragma warning disable CS0649 //whining about not assigning menus

public class GameManager : MonoBehaviour
{
    public static GameObject menu;
    public static World world;
    public static GameManager instance;

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
    [Tooltip("Only enable for a BUILD.")]
    public bool analyticsEnabled = true;

    //These determine how far the soul/skeleton can move from the player
    public float soulDistanceX;
    public float soulDistanceY;

    private bool gameActive = false;
    public bool isPaused = false;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject pauseQuitMenu;
    [SerializeField]
    public GameObject deathMenu;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject inputMenu;

    public AudioMixer masterMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Slider brightnessSlider;

    public AudioClip gameMusic;
    public AudioClip menuMusic;
    private bool toimiVittuSaatana = false;
    
    private Human human;

    public GameObject deathBox;
    public GameObject[] levels;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");

        levels = Resources.LoadAll<GameObject>("Prefabs/World/Master");
    }

    void Start()
    {
        //Options.Start();
        //ActivateMenu("MainMenu");
        instance = this;
        world = Instantiate(Resources.Load<GameObject>("Prefabs/World/World"), transform).GetComponent<World>();

        if (PlayerPrefs.HasKey("FirstRun") == false)
        {
            PlayerPrefs.SetInt("FirstRun", 0);

            PlayerPrefs.SetFloat("MasterVol", 0.5f);
            PlayerPrefs.SetFloat("MusicVol", 0.5f);
            PlayerPrefs.SetFloat("SFXVol", 0.5f);
            PlayerPrefs.SetFloat("Brightness", 0.5f);
            PlayerPrefs.SetString("Chunk_Ch1_Part1", "");
            masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
            SetAudio();
            PlayerPrefs.Save();
        }

        else
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVol");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVol");
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVol");
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
            PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
            PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
            PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
            PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
            SetAudio();
            PlayerPrefs.Save();
        }

        AudioManager.SetAmbiance(menuMusic);
    }

    private void Update()
    {
        PauseMenu();
        SetAudio(); //currently sets audio constantly, change later
    }

    private void CreateOutOfBoundsBox()
    {
        Instantiate(deathBox);
    }

    public void OptionsMenu()
    {
        if (gameActive == true)
        {
            pauseMenu.SetActive(true);
        }

        else
        {
            mainMenu.SetActive(true);
        }
    }

    private void PauseMenu()
    {
        if (Input.GetKeyDown(InputManager.instance.menu) && gameActive == true && human.state != "Dead")
        {
            if (pauseMenu.activeSelf == true || optionsMenu.activeSelf == true || pauseQuitMenu.activeSelf == true || inputMenu.activeSelf == true) //could add backing out feature later
            {
                pauseMenu.SetActive(false);
                optionsMenu.SetActive(false);
                pauseQuitMenu.SetActive(false);
                inputMenu.SetActive(false);
                Time.timeScale = 1;
                Time.fixedDeltaTime = 0.016667f;
                isPaused = false;
            }

            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0;
                Time.fixedDeltaTime = 0;
                isPaused = true;
            }
        }
    }

    public void SetAudio()
    {
        masterMixer.SetFloat("MasterVol", Mathf.Log10(masterSlider.value) * 20);
        masterMixer.SetFloat("MusicVol", Mathf.Log10(musicSlider.value) * 20);
        masterMixer.SetFloat("SFXVol", Mathf.Log10(sfxSlider.value) * 20);

        RenderSettings.ambientLight = new Color(brightnessSlider.value, brightnessSlider.value, brightnessSlider.value, 1f);
    }

    public GameObject GetMenu(string menuName)
    {
        switch (menuName)
        {
            case "MainMenu": return mainMenu;
            default: return null;
        }
    }

    public static bool MenuActive(string menuName)
    {
        if (menu?.name == $"{menuName}(Clone)")
            return true;
        return false;
    }

    public void SaveOptions()
    {
        //Options.SaveData();
        PlayerPrefs.SetFloat("MasterVol", masterSlider.value);
        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVol", sfxSlider.value);
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        PlayerPrefs.Save();
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
        //Debug.Log(chunk);
        World.Restart();
    }

    public void Continue()
    {
        if(deathMenu.activeSelf == true || toimiVittuSaatana == true)
        {
            SaveAnalytics();
            World.Restart();
            toimiVittuSaatana = false;
            AudioManager.SetAmbiance(gameMusic);
        }

        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.016667f;
    }

    public void DeathMenu()
    {
        deathMenu.SetActive(true);
        toimiVittuSaatana = true;

        //placeholder/testing crap
        GameObject testi = GameObject.Find(gameMusic.name);
        if (testi != null)
            Destroy(testi);
    }

    public void SetGameActive(bool isActive)
    {
        AudioManager.SetAmbiance(gameMusic);
        Time.timeScale = 1;
        Time.fixedDeltaTime = 0.016667f;
        gameActive = isActive;
        if (gameActive == false)
        {
            AudioManager.SetAmbiance(menuMusic);
            World.Remove();
        }
    }

    public void CheckAvailableLevels()
    {
        GameObject[] levelbuttons = GameObject.FindGameObjectsWithTag("levelButton");

        foreach (GameObject button in levelbuttons)
        {
            if(PlayerPrefs.HasKey(button.name))
            {
                button.GetComponent<Button>().interactable = true;
            }
        }
    }

    public void GetRealStartTime(float time)    //This for getting the correct start time after main menu
    {
        realStartTime = time;
        human = GameObject.Find("Human(Clone)").GetComponent<Human>();
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

            AnalyticsResult ar;

            AnalyticsEvent.Custom("Deaths", new Dictionary<string, object>
            {
                {"Deaths", deaths}
            });

            foreach (GameObject level in levels)
            {
                if(PlayerPrefs.HasKey(level.name))
                {
                    AnalyticsEvent.LevelComplete(level.name);
                    ar = AnalyticsEvent.LevelComplete(level.name);
                    Debug.Log("LevelComplete " + ar.ToString());
                }
            }

            ar = AnalyticsEvent.Custom("TimeSpentAs");
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