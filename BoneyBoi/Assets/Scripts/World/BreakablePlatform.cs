﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    public GameObject particle;
    public bool isBridge;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Box" && collision.relativeVelocity.y < -1.0f)
        {
            Break();
        }

        if (isBridge)
        {
            Instantiate(particle, this.transform);
        }


    }

    void Break()
    {
        GetComponent<Collider2D>().enabled = false;
        foreach (Transform child in transform)
            if (child.name != "bace")
            {
                Rigidbody2D rig = child.gameObject.AddComponent<Rigidbody2D>();
                rig.velocity = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-2.0f, 2.0f));
                rig.angularVelocity = Random.Range(-30.0f, 30.0f);
                child.gameObject.AddComponent<DeathTime>().SetDuration(Random.Range(1.0f, 2.0f));
            }
    }
}