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
    private GameObject player;

    private void Start()
    {
         player = GameObject.Find("Human");
    }

    private void LateUpdate()
    {
        if(deathBoxType == deathBoxEnum.OutOfBounds)
        {
            transform.position = new Vector3(player.transform.position.x, -5f);
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
                    //AudioManager.CreateAudio(fire, false, true, this.transform);

                    GameObject ash = Resources.Load<GameObject>("Prefabs/Ash2");
                    Instantiate(ash, collision.gameObject.transform.position + new Vector3(0,1.5f,0), transform.rotation);
                    SpriteRenderer[] sprites;
                    sprites = collision.gameObject.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sprite in sprites)
                        sprite.enabled = false;
                }
                break;
        }
    }
}
