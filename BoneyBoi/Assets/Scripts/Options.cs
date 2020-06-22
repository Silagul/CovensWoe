using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class Options
{
    public static string path = $"{Application.dataPath}/SaveData/Options";
    public static Stream stream;
    public static IFormatter formatter = new BinaryFormatter();
    public static OptionsData optionsData;

    public static void Start()
    {
        if (!Directory.Exists($"{Application.dataPath}/SaveData"))
            Directory.CreateDirectory($"{Application.dataPath}/SaveData");
        if (File.Exists(path))
            LoadData();
        else
            CreateData();
        AudioListener.volume = optionsData.volume;
        RenderSettings.ambientLight = Color32.Lerp(Color.black, Color.white, optionsData.brightness);
    }

    public static void CreateData()
    {
        optionsData = new OptionsData();
        SaveData();
    }

    public static void SaveData()
    {
        stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        formatter.Serialize(stream, optionsData);
        stream.Close();
    }

    public static void LoadData()
    {
        stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        optionsData = (OptionsData)formatter.Deserialize(stream);
        stream.Close();
    }
}

[System.Serializable]
public class OptionsData
{
    public float volume = 1.0f;
    public float brightness = 1.0f;
}