using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignManager : MonoBehaviour
{
    public static DesignManager Instance { get; private set; } = null;


    [SerializeField] BattleResource[] battleResources = null;
    [SerializeField] GameObject placeHolderPrefab = null;

    [SerializeField] DesignUI designUI = null;

    int budget = 0;
    int zoneWidth = 0;
    int zoneHeight = 0;
    string[,] battleZoneInfo = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    internal void RemovePlacedTile(Vector3 position, int cost)
    {
        budget += cost;
        designUI.SetBudgetText(budget);

        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.CeilToInt(position.y);

        battleZoneInfo[x, y] = ((int)TileCode.Free).ToString();
    }

    private void Start()
    {
        SetUpBattleZone(8, 8, 1000, battleResources);
    }

    public void SetUpBattleZone(int width, int height, int budget, BattleResource[] resources)
    {
        this.budget = budget;
        zoneWidth = width;
        zoneHeight = height;
        battleZoneInfo = new string[width, height];

        designUI.Init(budget, resources);
        GameObject placeHolderParent = new GameObject("PlaceHolders");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                battleZoneInfo[x, y] = ((int)TileCode.Free).ToString();
                Instantiate(placeHolderPrefab, Vector2.right * x + Vector2.up * y, Quaternion.identity, placeHolderParent.transform);
            }
        }
        Camera.main.GetComponent<CameraController>().SetPositionCenter(width, height);
    }

    public void TryPlacement(TileCode tileCode, int cost, Sprite icon, Vector3 position)
    {
        //Check coordinates
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.CeilToInt(position.y);

        if (x < 0 || x >= zoneWidth || y < 0 || y >= zoneHeight) return;

        //check existing position is empty
        if (int.Parse(battleZoneInfo[x, y]) != (int)TileCode.Free) return;

        //Check budget
        if (budget - cost < 0) return;

        budget -= cost;
        designUI.SetBudgetText(budget);
        battleZoneInfo[x, y] = ((int)tileCode).ToString();
        
        PlacementTile tile = new GameObject(tileCode.ToString()).AddComponent<PlacementTile>();
        tile.Icon = icon;
        tile.Cost = cost;
        tile.transform.position = new Vector2(x, y);
    }
}
