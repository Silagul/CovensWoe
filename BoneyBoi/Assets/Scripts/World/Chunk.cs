using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public static string currentChunk = "Chunk_Start(Clone)";
    public string[] neighbours = new string[] { };
    void Start()
    {
        World.loadlist.AddRange(neighbours);
    }

    public void Reload()
    {
        if (name == currentChunk)
            Activate();
        else
        {
            bool isNeighbour = false;
            foreach (string neighbour in neighbours)
            {
                Chunk chunk = World.GetChunk(neighbour);
                if (chunk?.name == currentChunk)
                    isNeighbour = true;
            }
            if (isNeighbour)
                Deactivate();
            else
            {
                World.chunks.Remove(this);
                Destroy(gameObject);
            }
        }
    }

    void Activate()
    {
        foreach (string neighbour in neighbours)
            if (!World.ChunkExists(neighbour))
                World.chunks.Add(Instantiate(Resources.Load<GameObject>($"Prefabs/World/Chunk_{neighbour}")).GetComponent<Chunk>());
    }

    void Deactivate()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "Human")
        {
            currentChunk = name;
            Reload();
            for (int i = World.chunks.Count - 1; i >= 0; i--)
                if (World.chunks[i] != this)
                    World.chunks[i].Reload();
        }
    }
}