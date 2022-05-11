using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DesignManager : MonoBehaviour
{
    public static DesignManager Instance { get; private set; } = null;
    
    public string[] players = null;
    [SerializeField] GameObject corePrefab = null;
    [SerializeField] BattleResource[] battleResources = null;
    [SerializeField] GameObject placeHolderPrefab = null;

    [SerializeField] DesignUI designUI = null;

    GameObject placeHoldersParent = null;
    GameObject tilesPlacedParent = null;
    GameConfig currentConfig = null;
    int currentBudget = 0;
    int currentZoneWidth = 0;
    int currentzoneHeight = 0;
    private int currentPlayerIndex = -1;
    Dictionary<string, string[,]> playersZoneInfo = null;

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
        currentBudget += cost;
        designUI.SetBudgetText(currentBudget);

        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);

        playersZoneInfo[players[currentPlayerIndex]][x, y] = ((int)TileCode.Free).ToString();
    }

    private void Start()
    {
        SetupDesign(GameConfig.Instance, battleResources);
    }

    public void SetupDesign(GameConfig config, BattleResource[] resources)
    {
        designUI.Init(resources);
        currentConfig = config;
        playersZoneInfo = new Dictionary<string, string[,]>();

        SetupPlayerDesign(0);
    }
    

    private void SetupPlayerDesign(int playerIndex)
    {
        currentPlayerIndex = playerIndex;

        currentBudget = currentConfig.Budget;
        currentZoneWidth = currentConfig.ZoneWidth;
        currentzoneHeight = currentConfig.ZoneHeight;
        playersZoneInfo[players[currentPlayerIndex]] = new string[currentZoneWidth, currentzoneHeight];

        designUI.SetBudgetText(currentBudget);
        designUI.SetPlayerText(players[currentPlayerIndex]);

        if (placeHoldersParent != null)
            Destroy(placeHoldersParent);

        if (tilesPlacedParent != null)
            Destroy(tilesPlacedParent);

        tilesPlacedParent = new GameObject("Tiles Placed");
        placeHoldersParent = new GameObject("PlaceHolders");
        for (int x = 0; x < currentZoneWidth; x++)
        {
            for (int y = 0; y < currentzoneHeight; y++)
            {
                playersZoneInfo[players[currentPlayerIndex]][x, y] = ((int)TileCode.Free).ToString();
                Instantiate(placeHolderPrefab, Vector2.right * x + Vector2.up * y, Quaternion.identity, placeHoldersParent.transform);
            }
        }
        int coreX = 1;
        int coreY = currentzoneHeight / 2;
        playersZoneInfo[players[currentPlayerIndex]][coreX, coreY] = ((int)TileCode.Core).ToString();
        Instantiate(corePrefab, Vector2.right * coreX + Vector2.up * coreY, Quaternion.identity, tilesPlacedParent.transform);
        Camera.main.GetComponent<CameraController>().SetPositionCenter(currentZoneWidth, currentzoneHeight);
    }

    public void TryPlacement(TileCode tileCode, int cost, Sprite icon, Vector3 position)
    {
        //Check coordinates
        int x = Mathf.RoundToInt(position.x);
        int y = Mathf.RoundToInt(position.y);
        
        if (x < 0 || x >= currentZoneWidth || y < 0 || y >= currentzoneHeight) return;

        //check existing position is empty
        if (int.Parse(playersZoneInfo[players[currentPlayerIndex]][x, y]) != (int)TileCode.Free) return;

        //Check budget
        if (currentBudget - cost < 0) return;

        currentBudget -= cost;
        designUI.SetBudgetText(currentBudget);
        playersZoneInfo[players[currentPlayerIndex]][x, y] = ((int)tileCode).ToString();

        PlacementTile tile = new GameObject(tileCode.ToString()).AddComponent<PlacementTile>();
        tile.Icon = icon;
        tile.Cost = cost;
        tile.transform.position = new Vector2(x, y);
        tile.transform.SetParent(tilesPlacedParent.transform);
    }

    public void Ready()
    {
        if(currentPlayerIndex == players.Length - 1)
        {
            SceneManager.sceneLoaded += OnBattleScene_sceneLoaded;
            SceneManager.LoadScene("BattleScene");
        }
        else
        {
            currentPlayerIndex++;
            SetupPlayerDesign(currentPlayerIndex);
        }
    }

    private void OnBattleScene_sceneLoaded(Scene sceneLoaded, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnBattleScene_sceneLoaded;

        if(sceneLoaded.name == "BattleScene")
        {
            GameObject.FindObjectOfType<BattleManager>().SetUpBattle(currentConfig, players[0], players[1], playersZoneInfo);
        }
    }
}
