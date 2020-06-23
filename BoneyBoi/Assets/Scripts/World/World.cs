using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class World : MonoBehaviour
{
    public static List<Chunk> chunks = new List<Chunk>();
    public static readonly float renderDistance = 48.0f;
    void Start()
    {
        chunks.Clear();
        chunks.Add(Instantiate(Resources.Load<GameObject>($"Prefabs/World/Chunk_Start"), transform).GetComponent<Chunk>());
        Chunk.currentChunk = chunks[0].name;
        chunks[0].Reload();
    }

    public static Chunk GetChunk(string chunkName)
    {
        foreach (Chunk chunk in chunks)
            if (chunk != null && chunk.name == $"Chunk_{chunkName}(Clone)")
                return chunk;
        return null;
    }

    public static bool ChunkExists(string chunkName)
    {
        foreach (Chunk chunk in chunks)
            if (chunk != null && chunk.name == $"Chunk_{chunkName}(Clone)")
                return true;
        return false;
    }
}