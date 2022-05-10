using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject placeHolderPrefab = null;
    [SerializeField] GameObject corePrefab = null;
    [SerializeField] GameObject unitAPrefab = null;
    [SerializeField] GameObject unitDPrefab = null;
    [SerializeField] GameObject heroPrefab = null;
    [SerializeField] GameObject t1Prefab = null;
    [SerializeField] GameObject t2Prefab = null;
    [SerializeField] GameObject rockPrefab = null;
    [SerializeField] GameObject concretePrefab = null;

    [SerializeField] GameObject grassPrefab = null;

    Dictionary<string, GameObject> tilesPrefabByCode = null;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        tilesPrefabByCode = new Dictionary<string, GameObject>()
        {
            {((int)TileCode.Core).ToString(), corePrefab },
            {((int)TileCode.Unit_Soldier_A).ToString(), unitAPrefab},
            {((int)TileCode.Unit_Soldier_D).ToString(), unitDPrefab},
            {((int)TileCode.Unit_Hero).ToString(), heroPrefab},
            {((int)TileCode.Turret_T1).ToString(), t1Prefab},
            {((int)TileCode.Turret_T2).ToString(), t2Prefab},
            {((int)TileCode.Wall_Rock).ToString(), rockPrefab},
            {((int)TileCode.Wall_Concrete).ToString(), concretePrefab},
        };
    }

    public void Generate(string[,] tileMap)
    {
        int width = tileMap.GetLength(0);
        int height = tileMap.GetLength(1);

        GameObject tilesParent = new GameObject("Battlefield");
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                string tileCode = tileMap[x, y];
                Vector2 pos = Vector2.right * x + Vector2.up * y;
                if (tileCode != ((int)TileCode.Free).ToString())
                {
                    GameObject prefab = tilesPrefabByCode[tileCode];
                    
                    if (tileCode == ((int)TileCode.Unit_Soldier_A).ToString() ||
                        tileCode == ((int)TileCode.Unit_Soldier_D).ToString() ||
                        tileCode == ((int)TileCode.Unit_Hero).ToString())
                    {
                        UnitSpawner spawner = new GameObject("Unit Spawner").AddComponent<UnitSpawner>();
                        spawner.transform.position = pos;
                        spawner.prefab = prefab;
                    }
                    else
                    {
                        Instantiate(prefab, pos, Quaternion.identity, tilesParent.transform);
                    }
                }
                Instantiate(placeHolderPrefab, pos, Quaternion.identity, tilesParent.transform);
            }
        }
    }

    [ContextMenu("Instantiate Grid")]
    public void CreateGrid()
    {
        GameObject backTilesParent = new GameObject("back tiles");
        for (int x = 0; x < 50; x++)
        {
            for (int y = 0; y < 50; y++)
            {
                Instantiate(grassPrefab, Vector2.right * x + Vector2.up * y, Quaternion.identity, backTilesParent.transform);
            }
        }
    }
}
