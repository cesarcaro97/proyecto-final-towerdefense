using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleLevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject player1UnitIndicator = null;
    [SerializeField] GameObject player2UnitIndicator = null;
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

    public void GenerateComplete(GameConfig config, string[,] tileMap, string player1, string player2)
    {
        int width = tileMap.GetLength(0);
        int height = tileMap.GetLength(1);

        GameObject tilesParent = new GameObject("Battlefield");

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                string forPlayer = x < width / 2 ? player1 : player2;
                //int layer = forPlayer == player1 ? 12 : 13;
                string ownTag = forPlayer == player1 ? "Player1Unit" : "Player2Unit";
                string enemyTag = forPlayer == player1 ? "Player2Unit" : "Player1Unit";
                GameObject indicatorPrefab = forPlayer == player1 ? player1UnitIndicator : player2UnitIndicator;

                TileCode tileCode = (TileCode)int.Parse(tileMap[x, y]);
                Vector2 pos = Vector2.right * x + Vector2.up * y;
                if (tileCode != TileCode.Free)
                {
                    GameObject prefab = tilesPrefabByCode[((int)tileCode).ToString()];
                    
                    if (tileCode == TileCode.Unit_Soldier_A ||
                        tileCode == TileCode.Unit_Soldier_D ||
                        tileCode == TileCode.Unit_Hero)
                    {
                        UnitSpawner spawner = new GameObject("Unit Spawner").AddComponent<UnitSpawner>();
                        //spawner.unitsLayer = layer;
                        spawner.unitSpawnTime = config.UnitSpawnTime;
                        spawner.transform.position = pos;
                        spawner.forPlayer = forPlayer;
                        spawner.enemyTag = enemyTag;
                        spawner.unitTag = ownTag;
                        spawner.prefab = prefab;
                        spawner.unitIndicatorPrefab = indicatorPrefab;
                    }
                    else if(tileCode == TileCode.Turret_T1 || tileCode == TileCode.Turret_T2)
                    {
                        TurretController turret = Instantiate(prefab, pos, Quaternion.identity).GetComponent<TurretController>();
                        turret.tag = ownTag;
                        turret.enemyTag = enemyTag;
                        turret.forPlayer = forPlayer;

                        Instantiate(indicatorPrefab, turret.transform);

                        PathFindManager.Instance.RegistePlayerResource(forPlayer, turret.transform, BattleResourceType.Turret);
                    }
                    else
                    {
                        //Walls y Core
                        IDestroyable d = Instantiate(prefab, pos, Quaternion.identity).GetComponent<IDestroyable>();
                        //d.GameObject.layer = layer;
                        d.GameObject.tag = ownTag;
                        d.ForPlayer = forPlayer;

                        Instantiate(indicatorPrefab, d.GameObject.transform);

                        if(tileCode == TileCode.Core)
                        {
                            PathFindManager.Instance.RegistePlayerResource(forPlayer, d.GameObject.transform, BattleResourceType.Core);
                        }
                        else
                        {
                            PathFindManager.Instance.RegistePlayerResource(forPlayer, d.GameObject.transform, BattleResourceType.Wall);
                        }
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
