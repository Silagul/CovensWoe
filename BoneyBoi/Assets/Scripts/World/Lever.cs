using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public Vector3 startRotation;
    public Vector3 endRotation;
    bool isRight = false;
    float time;
    float duration;
    float weight = 0.0f;

    public AudioClip leverAudio;
    public GameObject target;
    public Skeleton user;
    System.Action leverAction;
    Vector3 startPosition;
    float offset;

    void Update()
    {
        leverAction?.Invoke();
    }

    public override void Interact(Creature creature)
    {
        Skeleton skeleton;
        if (creature.TryGetComponent(out skeleton))
        {
            if (skeleton.isActive && Input.GetKey(InputManager.instance.interact) && leverAction == null)
            {
                time = 0.0f;
                duration = 0.25f;
                isRight = !isRight;
                leverAction = AwaitAction;
                AudioManager.CreateAudio(leverAudio, false, true, transform);
                startPosition = skeleton.transform.position;
                if (skeleton.transform.position.x > transform.position.x)
                {
                    offset = 1.5f;
                    skeleton.transform.localScale = new Vector3(0.15f, 0.15f, 1.0f);
                    if (isRight) skeleton.GetComponent<Animator>().Play("PullLever");
                    else skeleton.GetComponent<Animator>().Play("PushLever");
                }
                else
                {
                    offset = -1.5f;
                    skeleton.transform.localScale = new Vector3(-0.15f, 0.15f, 1.0f);
                    if (isRight) skeleton.GetComponent<Animator>().Play("PushLever");
                    else skeleton.GetComponent<Animator>().Play("PullLever");
                }
                skeleton.horizontal = 0.0f;
                skeleton.isActive = false;
                user = skeleton;
            }
        }
    }

    //Set user location to match animation
    void AwaitAction()
    {
        time += Time.deltaTime;
        user.transform.position = Vector3.Lerp(startPosition, transform.position + new Vector3(offset, 0), time / duration) ;
        if (time > duration)
        {
            time -= duration;
            duration = 0.4f;
            leverAction = CommitAction;
        }
    }

    void CommitAction()
    {
        time += Time.deltaTime;
        if (isRight) { weight = Mathf.Max(Mathf.Min(weight + Time.deltaTime / duration, 1.0f), 0.0f); }
        else { weight = Mathf.Max(Mathf.Min(weight - Time.deltaTime / duration, 1.0f), 0.0f); }
        transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, weight);
        if (time > duration)
        {
            Backforth backforth;
            if (target.TryGetComponent(out backforth))
                backforth.Activate();
            user.isActive = true;
            leverAction = null;
        }
    }
}