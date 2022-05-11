using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using System.Linq;
using System;
using PathFindingGrid = PathFinding.Grid;

public class PathFindManager : MonoBehaviour
{
    public static PathFindManager Instance { get; private set; }

    public Dictionary<string, Point> coreByPlayer = null;
    public Dictionary<string, List<Point>> turretsByPlayer = null;
    public Dictionary<string, List<Point>> wallsByPlayer = null;

    public Dictionary<string, List<Transform>> unitsByPlayer = null;

    string[] players;
    bool[,] completeMap = null;

    PathFindingGrid pathFindingGrid = null;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }


    public void Init(string player1, string player2, string[,] completeTileMap)
    {
        coreByPlayer = new Dictionary<string, Point>();
        turretsByPlayer = new Dictionary<string, List<Point>>() { { player1, new List<Point>() }, { player2, new List<Point>() } };
        wallsByPlayer = new Dictionary<string, List<Point>>() { { player1, new List<Point>() }, { player2, new List<Point>() } };
        unitsByPlayer = new Dictionary<string, List<Transform>>() { { player1, new List<Transform>() }, { player2, new List<Transform>() } };

        players = new string[] { player1, player2 };

        int width = completeTileMap.GetLength(0);
        int height = completeTileMap.GetLength(1);
        
        completeMap = new bool[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                TileCode tileCode = (TileCode)int.Parse(completeTileMap[x, y]);

                bool pos = tileCode == TileCode.Free ||
                                    tileCode == TileCode.Unit_Soldier_A ||
                                    tileCode == TileCode.Unit_Soldier_D ||
                                    tileCode == TileCode.Unit_Hero;

                completeMap[x, y] = pos;
                
                string forPlayer = x < width / 2 ? player1 : player2;
                Point p = new Point(x, y);
                switch (tileCode)
                {
                    case TileCode.Turret_T1:
                    case TileCode.Turret_T2:
                        turretsByPlayer[forPlayer].Add(p);
                        break;
                    case TileCode.Wall_Rock:
                    case TileCode.Wall_Concrete:
                        wallsByPlayer[forPlayer].Add(p);
                        break;
                    case TileCode.Core:
                        coreByPlayer.Add(forPlayer, p);
                        break;
                    default:
                        break;
                }
            }
        }
        pathFindingGrid = new PathFindingGrid(completeMap);
    }

    public void RegisterPlayerUnit(string player, Transform unit)
    {
        unitsByPlayer[player].Add(unit);
    }

    public void UnRegisterPlayerUnit(string player, Transform unit)
    {
        unitsByPlayer[player].Remove(unit);
    }

    public Vector2 GetFirstPathPoint(Vector2 from, Vector2 to)
    {
        Point f = new Point((int)from.x, (int)from.y);
        Point t = new Point((int)to.x, (int)to.y);
        Point p = Pathfinding.FindPath(pathFindingGrid, f, t, Pathfinding.DistanceType.Manhattan).FirstOrDefault();

        return new Vector2(p.x, p.y);
    }
}
