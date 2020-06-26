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
        name = name.Substring(0, name.Length - 7);
    }

    public static void Restart()
    {
        Creature.dying = false;
        Creature.visibleTime = 0.0f;
        Remove();
        chunks.Add(Instantiate(Resources.Load<GameObject>($"Prefabs/World/Master/{Chunk.currentChunk}"), GameManager.world.transform).GetComponent<Chunk>());
        GameObject human = Instantiate(Resources.Load<GameObject>("Prefabs/Human"), GameManager.world.transform);
        human.transform.position = chunks[0].transform.TransformPoint(chunks[0].localSpawnPoint);
        chunks[0].Reload();
    }

    public static void Remove()
    {
        foreach (Transform t in GameManager.world.transform)
            Destroy(t.gameObject);
        chunks.Clear();
    }

    public static Chunk GetChunk(string chunkName)
    {
        foreach (Chunk chunk in chunks)
            if (chunk != null && chunk.name == $"Chunk_{chunkName}")
                return chunk;
        return null;
    }

    public static bool ChunkExists(string chunkName)
    {
        foreach (Chunk chunk in chunks)
            if (chunk != null && chunk.name == $"Chunk_{chunkName}")
                return true;
        return false;
    }
}