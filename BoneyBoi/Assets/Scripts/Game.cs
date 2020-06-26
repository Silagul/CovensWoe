using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static GameObject menu;
    public static World world;
    void Start()
    {
        Options.Start();
        ActivateMenu("MainMenu");
        world = Instantiate(Resources.Load<GameObject>("Prefabs/World/World"), transform).GetComponent<World>();
    }

    public static void ActivateMenu(string menuName)
    {
        if (menu != null)
            Destroy(menu);
        menu = Instantiate(Resources.Load<GameObject>($"Prefabs/UI/{menuName}"), Camera.main.transform);
    }

    public static bool MenuActive(string menuName)
    {
        if (menu?.name == $"{menuName}(Clone)")
            return true;
        return false;
    }
}