using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public string[] neighbours = new string[] { };
    void Update()
    {
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 chunkPosition = transform.position;
        float distance = Vector2.Distance(new Vector2(cameraPosition.x, cameraPosition.y), new Vector2(chunkPosition.x, chunkPosition.y));
        if (distance >= 48.0f)
        {
            World.chunks.Remove(gameObject);
            Destroy(gameObject);
        }
    }

}