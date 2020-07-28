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
    static Color32 activeColor = new Color32(255, 100, 100, 255);
    static Color32 deactiveColor = new Color32(100, 100, 100, 255);

    public AudioClip[] mushroomAudioArray;
    public AudioClip mushroomTimer;
    public AudioClip hiddenMushroomAudio;
    public List<SpriteRenderer> mushroomPatches;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.tag == "Player" && isActive == false)
        if(collision.transform.parent.name == "Skeleton" && isActive == false)
        {
            Interact(collision.GetComponent<Creature>());
            AudioManager.CreateAudio(mushroomTimer, true, false, transform);
            AudioManager.CreateAudio(mushroomAudioArray[Random.Range(0, mushroomAudioArray.Length)], false, true, transform);
        }

        else if (collision.transform.parent.name == "Human")
        {
            AudioManager.CreateAudio(hiddenMushroomAudio, false, true, transform);
        }
    }

    void Start()
    {
        GetComponent<SpriteRenderer>().color = deactiveColor;
        GetComponentInParent<SpriteRenderer>().color = deactiveColor;
        foreach (SpriteRenderer mushroomPatch in mushroomPatches)
            mushroomPatch.color = deactiveColor;
    }

    void FixedUpdate()
    {
        if (isActive && Time.time > startTime + maxDuration)
            foreach (TimeShroom shroom in timeShrooms)
            {
                shroom.GetComponent<SpriteRenderer>().color = deactiveColor;
                GetComponentInParent<SpriteRenderer>().color = deactiveColor;
                foreach (SpriteRenderer mushroomPatch in mushroomPatches)
                    mushroomPatch.color = deactiveColor;
                shroom.isActive = false;
                GameObject timerObject = GameObject.Find(mushroomTimer.name.Substring(0, mushroomTimer.name.Length - 1));
                Destroy(timerObject);
            }
    }

    public override void Interact(Creature creature)
    {
        isActive = true;
        GetComponent<SpriteRenderer>().color = activeColor;
        GetComponentInParent<SpriteRenderer>().color = activeColor;
        foreach (SpriteRenderer mushroomPatch in mushroomPatches)
            mushroomPatch.color = activeColor;
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