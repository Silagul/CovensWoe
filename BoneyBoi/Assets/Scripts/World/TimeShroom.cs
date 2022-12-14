using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeShroom : Interactable
{
    private void OnTriggerStay2D(Collider2D collision) { }

    public List<TimeShroom> timeShrooms = new List<TimeShroom>();
    public bool isActive = false;
    static float startTime = Mathf.NegativeInfinity;
    static float maxDuration = 10.0f;
    public Interactable interactable;

    public AudioClip[] mushroomAudioArray;
    public AudioClip mushroomTimer;
    public AudioClip hiddenMushroomAudio;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Player" && isActive == false)
        if(collision.name == "Skeleton" && isActive == false)
        {
            Interact(collision.GetComponent<Creature>());
            AudioManager.CreateAudio(mushroomTimer, true, false, transform);
            AudioManager.CreateAudio(mushroomAudioArray[Random.Range(0, mushroomAudioArray.Length)], false, true, transform);
        }

        else if (collision.name == "Human")
        {
            AudioManager.CreateAudio(hiddenMushroomAudio, false, true, transform);
        }
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color32(127, 127, 127, 255);
        GetComponentInParent<SpriteRenderer>().color = new Color32(127, 127, 127, 255);
    }

    void FixedUpdate()
    {
        if (isActive && Time.time > startTime + maxDuration)
            foreach (TimeShroom shroom in timeShrooms)
            {
                shroom.GetComponent<SpriteRenderer>().color = new Color32(127, 127, 127, 255);
                GetComponentInParent<SpriteRenderer>().color = new Color32(127, 127, 127, 255);
                shroom.isActive = false;
                GameObject timerObject = GameObject.Find(mushroomTimer.name.Substring(0, mushroomTimer.name.Length - 1));
                Destroy(timerObject);
            }
    }

    public override void Interact(Creature creature)
    {
        isActive = true;
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        GetComponentInParent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        if (Time.time > startTime + maxDuration)
            startTime = Time.time;
        bool hasDeactive = false;
        foreach (TimeShroom shroom in timeShrooms)
            if (!shroom.isActive)
            {
                hasDeactive = true;
                break;
            }

        if (!hasDeactive)
        {
            Debug.Log("Victory!");
            interactable.Interact(null);
            GameObject timerObject = GameObject.Find(mushroomTimer.name.Substring(0, mushroomTimer.name.Length - 1));
            Destroy(timerObject);
        }
    }
}