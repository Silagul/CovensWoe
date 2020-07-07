using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class TimeShroom : Interactable
{
    private void OnTriggerEnter2D(Collider2D collision) { if (collision.tag == "Player") Interact(collision.GetComponent<Creature>()); }
    private void OnTriggerStay2D(Collider2D collision) { }

    static TimeShroom[] timeShrooms = new TimeShroom[5];
    public int index;
    public bool isActive = false;
    static float startTime = Mathf.NegativeInfinity;
    static float maxDuration = 10.0f;

    public AudioClip[] mushroomAudioArray;
    public AudioClip mushroomAudio;

    void Start()
    {
        timeShrooms[index] = this;
        GetComponent<SpriteRenderer>().color = new Color32(127, 127, 127, 255);
    }

    void FixedUpdate()
    {
        if (isActive && Time.time > startTime + maxDuration)
            foreach (TimeShroom shroom in timeShrooms)
            {
                shroom.GetComponent<SpriteRenderer>().color = new Color32(127, 127, 127, 255);
                shroom.isActive = false;
            }
    }

    public override void Interact(Creature creature)
    {
        isActive = true;
        //AudioManager.CreateAudio(mushroomAudioArray[Random.Range(0, mushroomAudioArray.Length)], false, this.transform);
        GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        if (Time.time > startTime + maxDuration)
            startTime = Time.time;
        else
        {
            bool hasDeactive = false;
            foreach (TimeShroom shroom in timeShrooms)
                if (!shroom.isActive)
                {
                    hasDeactive = true;
                    //AudioManager.CreateAudio(mushroomAudio, false, this.transform);
                    Debug.Log("whyy");
                }

            if (!hasDeactive)
                Debug.Log("Victory!");
        }
    }
}