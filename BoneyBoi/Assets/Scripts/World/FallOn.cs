using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallOn : MonoBehaviour
{
    public AudioClip boxFallingAudio;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Human human;
        if (collision.TryGetComponent(out human))
        {
            if (GetComponentInParent<Rigidbody2D>().velocity.y < 0.0f && transform.position.y > collision.transform.position.y + (human.tag == "Hollow" ? 1.0f : 2.75f))
                if (GetComponentInParent<Platform>().floorCount == 0)
                    human.SetState("Dead");
        }
        else if (collision.tag == "Floor")
            AudioManager.CreateAudio(boxFallingAudio, false, true, this.transform);
    }

    public void FixedUpdate()
    {
        Vector3 rotation = transform.parent.eulerAngles;
        if (rotation.z < 180) { if (rotation.z > 40.0f) rotation.z = 40.0f; }
        else if (rotation.z < 320.0f) rotation.z = 320.0f;
        transform.parent.eulerAngles = rotation;
    }
}
