using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public static BattleManager Instance { get; private set; } = null;

    [SerializeField] BattleLevelGenerator levelGenerator = null;


    string player1 = string.Empty, player2 = string.Empty;
    string[,] player1TileMap;
    string[,] player2TileMap;
    private string[,] completeTileMap;
    GameConfig gameConfig = null;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


    public void SetUpBattle(GameConfig config, string player1, string player2, Dictionary<string, string[,]> playersTilesMap)
    {
        print("Set up battle");
        gameConfig = config;
        this.player1 = player1;
        this.player2 = player2;
        SetPlayersTilesMap(player1, player2, playersTilesMap);

        UnifyPlayersTilesMap(player1TileMap, player2TileMap);
        Camera.main.GetComponent<CameraController>().SetPositionCenter(completeTileMap.GetLength(0) / 2, completeTileMap.GetLength(1) / 2);

        PathFindManager.Instance.Init(player1, player2, completeTileMap);

        levelGenerator.GenerateComplete(gameConfig, completeTileMap, player1, player2);
    }

    private void SetPlayersTilesMap(string player1, string player2, Dictionary<string, string[,]> playersTilesMap)
    {
        player1TileMap = playersTilesMap[player1];

        //Perform transformation on player2 tilemap (Mirror like)
        int player2TileMapWidth = playersTilesMap[player2].GetLength(0);
        int player2TileMapHeight = playersTilesMap[player2].GetLength(1);
        
        player2TileMap = new string[player2TileMapWidth, player2TileMapHeight];
        for (int x = 0; x < player2TileMapWidth; x++)
        {
            for (int y = 0; y < player2TileMapHeight; y++)
            {
                player2TileMap[player2TileMapWidth - 1 - x, y] = playersTilesMap[player2][x, y];
            }
        }
    }

    private void UnifyPlayersTilesMap(string[,] player1TilesMap, string[,] player2TilesMap)
    {
        int completeWidth = player1TileMap.GetLength(0) + player2TileMap.GetLength(0);
        int height = player1TileMap.GetLength(1);
        
        completeTileMap = new string[completeWidth, height];

        int x = 0, y = 0;
        while (x < player1TileMap.GetLength(0))
        {
            while(y < player1TileMap.GetLength(1))
            {
                completeTileMap[x, y] = player1TileMap[x, y];
                y++;
            }
            y = 0;
            x++;
        }

        int player2TileMapWidth = player2TileMap.GetLength(0);
        while (x < completeWidth)
        {
            while(y < height)
            {
                int i = x - player2TileMapWidth;
                completeTileMap[x, y] = player2TileMap[i, y];
                y++;
            }
            y = 0;
            x++;
        }
    }

    public void GameOver(string losingPlayer)
    {
        if(losingPlayer == player1)
        {

        }
        else
        {

        }
    }
}
