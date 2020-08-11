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
            if (player != null)
            transform.position = new Vector3(player.transform.position.x, -10f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(deathBoxType)
        {
            case deathBoxEnum.SpikePit:
                if (collision.transform.parent.name == "Human")
                {
                    collision.transform.parent.GetComponent<Animator>().SetBool("Foothold", true);
                    collision.transform.parent.GetComponent<Human>().SetState("Dead");
                    AudioManager.CreateAudio(spikePit, false, true, this.transform);
                }
                break;
            case deathBoxEnum.OutOfBounds:
                if(collision.tag == "Player" || collision.transform.parent.name == "Human")
                {
                    GameObject.Find("Human").GetComponent<Human>().SetState("Dead");
                }
                //else if(collision.transform.parent.name == "Skeleton")
                //{
                //    Destroy(collision.gameObject);
                //}
                break;
            case deathBoxEnum.Fire:
                if (collision.transform.parent.name == "Human")
                {
                    collision.transform.parent.GetComponent<Animator>().SetBool("Foothold", true);
                    collision.transform.parent.GetComponent<Human>().SetState("Dead");
                    //AudioManager.CreateAudio(fire, false, true, this.transform);

                    GameObject ash = Resources.Load<GameObject>("Prefabs/Ash2");
                    Instantiate(ash, collision.transform.position + new Vector3(0,1.5f,0), transform.rotation);
                    SpriteRenderer[] sprites;
                    sprites = collision.GetComponentsInChildren<SpriteRenderer>();
                    foreach (SpriteRenderer sprite in sprites)
                        sprite.enabled = false;
                }
                break;
        }
    }
}
