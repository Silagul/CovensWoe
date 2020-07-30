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
    Skeleton user;
    Human user2;
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
        Human human;
        if (creature.TryGetComponent(out skeleton))
        {
            if (skeleton.isActive && !Movable.IsHolding() && Input.GetKey(InputManager.instance.interact) && leverAction == null)
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
        else if (creature.TryGetComponent(out human))
        {
            if (human.isActive && !Movable.IsHolding() && Input.GetKey(InputManager.instance.interact) && leverAction == null)
            {
                time = 0.0f;
                duration = 0.25f;
                isRight = !isRight;
                leverAction = AwaitActionHuman;
                AudioManager.CreateAudio(leverAudio, false, true, transform);
                startPosition = human.transform.position;
                if (human.transform.position.x > transform.position.x)
                {
                    offset = 1.5f;
                    human.transform.localScale = new Vector3(0.2f, 0.2f, 1.0f);
                    if (isRight) human.GetComponent<Animator>().Play("PullLever");
                    else human.GetComponent<Animator>().Play("PushLever");
                }
                else
                {
                    offset = -1.5f;
                    human.transform.localScale = new Vector3(-0.2f, 0.2f, 1.0f);
                    if (isRight) human.GetComponent<Animator>().Play("PushLever");
                    else human.GetComponent<Animator>().Play("PullLever");
                }
                human.horizontal = 0.0f;
                human.isActive = false;
                user2 = human;
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

    void AwaitActionHuman()
    {
        time += Time.deltaTime;
        user2.transform.position = Vector3.Lerp(startPosition, transform.position + new Vector3(offset, 0), time / duration);
        if (time > duration)
        {
            time -= duration;
            duration = 0.4f;
            leverAction = CommitActionHuman;
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
            time -= duration;
            duration = 0.517f;
            Backforth backforth;
            if (target.TryGetComponent(out backforth))
                backforth.Activate();
            leverAction = AwaitUserReset;
        }
    }

    void CommitActionHuman()
    {
        time += Time.deltaTime;
        if (isRight) { weight = Mathf.Max(Mathf.Min(weight + Time.deltaTime / duration, 1.0f), 0.0f); }
        else { weight = Mathf.Max(Mathf.Min(weight - Time.deltaTime / duration, 1.0f), 0.0f); }
        transform.eulerAngles = Vector3.Lerp(startRotation, endRotation, weight);
        if (time > duration)
        {
            time -= duration;
            duration = 0.517f;
            Backforth backforth;
            if (target.TryGetComponent(out backforth))
                backforth.Activate();
            leverAction = AwaitUserResetHuman;
        }
    }

    void AwaitUserReset()
    {
        time += Time.deltaTime;
        if (time > duration)
        {
            user.isActive = true;
            leverAction = null;
        }
    }

    void AwaitUserResetHuman()
    {
        time += Time.deltaTime;
        if (time > duration)
        {
            user2.isActive = true;
            leverAction = null;
        }
    }
}