using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static List<AudioManager> ambiance = new List<AudioManager>();
    AudioSource audioSource;
    System.Action fadeInOut;
    System.Action fadeWhile;
    Transform target;
    float time = 0.0f;
    float weight = 0.0f; //Fade in and out
    float overweight = 1.0f; //Volume fade (distance for now...)
    float duration;
    float maxDistance = 8.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        fadeInOut?.Invoke();
        fadeWhile?.Invoke();
        audioSource.volume = weight * overweight;
        time += Time.deltaTime;
        if (time > duration)
            Destroy(gameObject);
    }

    void FadeIn()
    {
        weight = Mathf.Min(weight + Time.deltaTime, 1.0f);
        if (weight == 1.0f)
            fadeInOut = null;
    }

    void FadeOut()
    {
        weight = Mathf.Max(weight - Time.deltaTime, 0.0f);
        if (weight == 0.0f)
            Destroy(gameObject);
    }

    void FadeWhile()
    {
        if (target != null) overweight = Mathf.Max(0.0f, 1.0f - Vector2.Distance(Camera.main.transform.position, target.position) / maxDistance);
        else Destroy(gameObject);
    }

    void OnDestroy()
    {
        ambiance.Remove(this);
    }

    ///Global audio uses Camera.main.transform as target, localized audio uses... whatever transform it is located at.
    public static AudioManager CreateAudio(AudioClip clip, bool loop, Transform target)
    {
        GameObject go = new GameObject(clip.name);
        go.transform.parent = Camera.main.transform;
        AudioSource audio = go.AddComponent<AudioSource>();
        AudioManager audioManager = go.AddComponent<AudioManager>();
        audio.clip = clip;
        audio.Play();
        if (loop)
        {
            audioManager.fadeInOut = audioManager.FadeIn;
            audioManager.duration = Mathf.Infinity;
        }
        else
        {
            audioManager.weight = 1.0f;
            audioManager.duration = clip.length;
        }
        if (target != Camera.main.transform)
            audioManager.fadeWhile = audioManager.FadeWhile;
        audioManager.target = target;
        return audioManager;
    }

    public static AudioManager SetAmbiance(AudioClip clip)
    {
        GameObject go = new GameObject(clip.name);
        go.transform.parent = Camera.main.transform;
        AudioSource audio = go.AddComponent<AudioSource>();
        AudioManager audioManager = go.AddComponent<AudioManager>();
        audio.clip = clip;
        audio.Play();
        audioManager.fadeInOut = audioManager.FadeIn;
        foreach (AudioManager ambiant in ambiance)
            ambiant.fadeInOut = ambiant.FadeOut;
        audioManager.duration = Mathf.Infinity;
        ambiance.Add(audioManager);
        return audioManager;
    }
}