﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static GameObject menu;
    void Start()
    {
        Options.Start();
        ActivateMenu("MainMenu");
    }

    public static void ActivateMenu(string menuName)
    {
        if (menu != null)
            Destroy(menu);
        menu = Instantiate(Resources.Load<GameObject>($"Prefabs/UI/{menuName}"), Camera.main.transform);
    }

    public static void Restart()
    {
        GameObject world = GameObject.Find("World(Clone)");
        if (world != null)
        {
            Destroy(world);
            Destroy(GameObject.Find("Human"));
            Destroy(GameObject.Find("Skeleton"));
            if (GameObject.Find("Soul(Clone)") != null)
                Destroy(GameObject.Find("Soul(Clone)"));
        }
        Creature.visibleTime = 0.0f;
        Instantiate(Resources.Load<GameObject>("Prefabs/World/World"));
    }

    public static bool MenuActive(string menuName)
    {
        if (menu?.name == $"{menuName}(Clone)")
            return true;
        return false;
    }
}