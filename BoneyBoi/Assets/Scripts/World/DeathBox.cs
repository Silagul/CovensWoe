using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBox : MonoBehaviour
{
    public enum deathBoxEnum
    {
        SpikePit,
        OutOfBounds,
        Fire
    };

    public deathBoxEnum deathBoxType;
    public AudioClip spikePit;
    //public AudioClip fire;

    private void Start()
    {
        switch(deathBoxType)
        {
            case deathBoxEnum.OutOfBounds:
                GetComponent<SpriteRenderer>().enabled = false;
                break;

            default:
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(deathBoxType)
        {
            case deathBoxEnum.SpikePit:
                if (collision.gameObject.name == "Human")
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("Foothold", true);
                    collision.gameObject.GetComponent<Human>().SetState("Dead");
                    AudioManager.CreateAudio(spikePit, false, true, this.transform);
                }
                break;
            case deathBoxEnum.OutOfBounds:
                if(collision.tag == "Player" || collision.gameObject.name == "Human")
                {
                    GameObject.Find("Human").GetComponent<Human>().SetState("Dead");
                }
                break;
            case deathBoxEnum.Fire:
                if (collision.gameObject.name == "Human")
                {
                    collision.gameObject.GetComponent<Animator>().SetBool("Foothold", true);
                    collision.gameObject.GetComponent<Human>().SetState("Dead");
                    //do particle effect here
                    //AudioManager.CreateAudio(fire, false, true, this.transform);
                }
                break;
        }
    }
}
