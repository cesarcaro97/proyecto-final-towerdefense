using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

[Serializable]
public class GameConfig
{
    static GameConfig instance = null;
    public static GameConfig Instance
    {
        get
        {
            if (instance == null) instance = LoadFromFile();

            return instance;
        }
    }

    const int DEFAULT_BUDGET = 1000;
    const int DEFAULT_ZONE_WIDTH = 8;
    const int DEFAULT_ZONE_HEIGHT = 9;

    [SerializeField] int budget = DEFAULT_BUDGET;
    [SerializeField] int zoneWidth = DEFAULT_ZONE_WIDTH;
    [SerializeField] int zoneHeight = DEFAULT_ZONE_HEIGHT;

    public int ZoneWidth => zoneWidth;
    public int ZoneHeight => zoneHeight;
    public int Budget => budget;

    private static GameConfig LoadFromFile()
    {
        GameConfig config = null;
#if UNITY_EDITOR
        config = new GameConfig();
#else
        string filePath = Path.Combine(Application.dataPath, "gameconfig.json");
        try
        {
            if (File.Exists(filePath))
            {
                config = JsonUtility.FromJson<GameConfig>(File.ReadAllText(filePath, Encoding.UTF8).Trim());
            }
            else
            {
                config = new GameConfig();
                File.WriteAllText(filePath, JsonUtility.ToJson(config, true), Encoding.UTF8);
            }
        }
        catch (Exception)
        {
            config = new GameConfig();
        }
        
#endif
        return config;
    }
}
