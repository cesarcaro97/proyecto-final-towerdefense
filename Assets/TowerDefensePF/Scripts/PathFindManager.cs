using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathFinding;
using System.Linq;
using System;
using PathFindingGrid = PathFinding.Grid;
using Random = UnityEngine.Random;

public class PathFindManager : MonoBehaviour
{
    public static PathFindManager Instance { get; private set; }

    public Dictionary<string, Transform> coreByPlayer = null;
    public Dictionary<string, List<Transform>> turretsByPlayer = null;
    public Dictionary<string, List<Transform>> wallsByPlayer = null;

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
        coreByPlayer = new Dictionary<string, Transform>();
        turretsByPlayer = new Dictionary<string, List<Transform>>() { { player1, new List<Transform>() }, { player2, new List<Transform>() } };
        wallsByPlayer = new Dictionary<string, List<Transform>>() { { player1, new List<Transform>() }, { player2, new List<Transform>() } };
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

                //string forPlayer = x < width / 2 ? player1 : player2;
                //Point p = new Point(x, y);
                //switch (tileCode)
                //{
                //    case TileCode.Turret_T1:
                //    case TileCode.Turret_T2:
                //        turretsByPlayer[forPlayer].Add(p);
                //        break;
                //    case TileCode.Wall_Rock:
                //    case TileCode.Wall_Concrete:
                //        wallsByPlayer[forPlayer].Add(p);
                //        break;
                //    case TileCode.Core:
                //        coreByPlayer.Add(forPlayer, p);
                //        break;
                //    default:
                //        break;
                //}
            }
        }
        pathFindingGrid = new PathFindingGrid(completeMap);
    }

    public void RegistePlayerResource(string player, Transform resource, BattleResourceType type)
    {
        switch (type)
        {
            case BattleResourceType.Unit:
                unitsByPlayer[player].Add(resource);
                break;
            case BattleResourceType.Turret:
                turretsByPlayer[player].Add(resource);
                break;
            case BattleResourceType.Wall:
                wallsByPlayer[player].Add(resource);
                break;
            case BattleResourceType.Core:
                coreByPlayer[player] = resource;
                break;
            default:
                throw new NotImplementedException();
        }
    }

    public void UnregisterPlayerResource(string player, Transform resource, BattleResourceType type)
    {
        switch (type)
        {
            case BattleResourceType.Unit:
                unitsByPlayer[player].Remove(resource);
                break;
            case BattleResourceType.Turret:
                turretsByPlayer[player].Remove(resource);
                break;
            case BattleResourceType.Wall:
                wallsByPlayer[player].Remove(resource);
                break;
            case BattleResourceType.Core:
                coreByPlayer[player] = null;
                break;
            default:
                throw new NotImplementedException();
        }
        int x = (int)resource.position.x;
        int y = (int)resource.position.y;

        completeMap[x, y] = true;

        pathFindingGrid.UpdateGrid(completeMap);
    }


    public Vector2 GetFirstPathPoint(Vector2 from, Vector2 to)
    {
        Point f = new Point((int)from.x, (int)from.y);
        Point t = new Point((int)to.x, (int)to.y);
        Point p;
        try
        {
            p = Pathfinding.FindPath(pathFindingGrid, f, t, Pathfinding.DistanceType.Manhattan).First();
        }
        catch (Exception)
        {
            p = new Point(-1, -1);
        }

        return new Vector2(p.x, p.y);
    }


    private Point GetPointNextTo(Point point)
    {
        List<Point> points = new List<Point>();
        int mapWidth = completeMap.GetLength(0);
        int mapHeight = completeMap.GetLength(1);
        int x = point.x;
        int y = point.y;

        if (x > 0 && completeMap[x - 1, y] == true)
            points.Add(new Point(x - 1, y));

        if(y < mapHeight - 1 && completeMap[x, y + 1]== true)
            points.Add(new Point(x, y + 1));

        if (x < mapWidth - 1 && completeMap[x + 1, y] == true)
            points.Add(new Point(x + 1, y));

        if (y > 0 && completeMap[x, y - 1] == true)
            points.Add(new Point(x, y - 1));

        if (points.Count > 0)
            return points[Random.Range(0, points.Count)];
        else
            return new Point(-1,-1);
    }

    public Vector2 GetPointNextTo(Vector2 point)
    {
        Point p = GetPointNextTo(new Point((int)point.x, (int)point.y));
        return new Vector2(p.x, p.y);
    }

    public Transform GetWallTarget(string player)
    {
        string enemyPlayer = player == players[0] ? players[1] : players[0];

        if (wallsByPlayer[enemyPlayer].Count > 0)
        {
            int random = Random.Range(0, wallsByPlayer[enemyPlayer].Count);
            var wall = wallsByPlayer[enemyPlayer][random];
            return wall;
        }
        else
        {
            return null;
        }
    }

    public Transform GetTurretTarget(string player)
    {
        string enemyPlayer = player == players[0] ? players[1] : players[0];

        if (turretsByPlayer[enemyPlayer].Count > 0)
        {
            int random = Random.Range(0, turretsByPlayer[enemyPlayer].Count);
            var turret = turretsByPlayer[enemyPlayer][random];
            return turret;
        }
        else
        {
            return null;
        }
    }

    public Transform GetCoreTarget(string player)
    {
        string enemyPlayer = player == players[0] ? players[1] : players[0];
        var core = coreByPlayer[enemyPlayer];

        return core;
    }

    public Transform GetUnitTarget(string player, TileCode unitType)
    {
        string enemyPlayer = player == players[0] ? players[1] : players[0];
        List<UnitController> units = new List<UnitController>();
        switch (unitType)
        {
            case TileCode.Unit_Soldier_A:
                
                break;
            case TileCode.Unit_Soldier_D:
                break;
            case TileCode.Unit_Hero:
                break;
            default:
                throw new ArgumentException("Tile code is not a unit type", nameof(unitType));
        }

        units = unitsByPlayer[enemyPlayer].Select(x => x.GetComponent<UnitController>()).Where(x => x.UnitType == unitType).ToList();

        if (units.Count > 0)
        {
            int random = Random.Range(0, units.Count);
            var unit = units[random];
            
            return unit.transform;
        }
        else
        {
            return null;
        }
    }
}
