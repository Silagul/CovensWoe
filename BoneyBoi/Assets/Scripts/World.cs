using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public static List<GameObject> chunks = new List<GameObject>();
    void Start()
    {
        chunks.Add(CreateChunk("Start", true));
    }

    void Update()
    {
        for (int i = chunks.Count - 1; i >= 0; i--)
        {
            if (chunks[i] != null)
            {
                string[] neighbours = chunks[i].GetComponent<Chunk>().neighbours;
                foreach (string neighbour in neighbours)
                    if (!ChunkExists(neighbour))
                    {
                        GameObject chunk = CreateChunk(neighbour, true);
                        if (chunk != null)
                            chunks.Add(chunk);
                    }
            }
        }
    }

    GameObject CreateChunk(string chunkName, bool checkForNeighbours)
    {
        GameObject chunk = Instantiate(Resources.Load<GameObject>($"Prefabs/Chunk_{chunkName}"));
        Vector3 cameraPosition = Camera.main.transform.position;
        Vector3 chunkPosition = chunk.transform.position;
        float distance = Vector2.Distance(new Vector2(cameraPosition.x, cameraPosition.y), new Vector2(chunkPosition.x, chunkPosition.y));
        if (distance < 48.0f)
        {
            string[] neighbours = chunk.GetComponent<Chunk>().neighbours;
            if (checkForNeighbours)
                foreach (string neighbour in neighbours)
                    if (!ChunkExists(neighbour))
                    {
                        GameObject c = CreateChunk(neighbour, false);
                        if (c != null)
                            chunks.Add(c);
                    }
        }
        else
            Destroy(chunk);
        return chunk;
    }

    bool ChunkExists(string chunkName)
    {
        foreach (GameObject chunk in chunks)
            if (chunk != null && chunk.name == $"Chunk_{chunkName}(Clone)")
                return true;
        return false;
    }
}