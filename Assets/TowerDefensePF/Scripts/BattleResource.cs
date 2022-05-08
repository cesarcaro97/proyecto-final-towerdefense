using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Resource", menuName = "Battle Resource", order = 0)]
public class BattleResource : ScriptableObject
{
    [SerializeField]
    private string _name = string.Empty;
    [SerializeField]
    [TextArea(4, 6)]
    private string description;
    [SerializeField]
    Stat[] statsInfo = null;
    [SerializeField]
    private Sprite icon = null;
    [SerializeField]
    private int cost = 0;
    [SerializeField]
    private BattleResourceType resourceType = BattleResourceType.Unit;
    [SerializeField]
    private TileCode tileCode = TileCode.Free;

    public string Name  => _name;
    public string Description => description;
    public Stat[] StatsInfo => statsInfo;
    public string TooltipInfo => $"{Name}{Environment.NewLine}{Environment.NewLine}" +
                                $"{Description}{Environment.NewLine}{Environment.NewLine}" +
                                $"Stats{Environment.NewLine}" +
                                $"{string.Join<Stat>(Environment.NewLine, statsInfo)}";
    public Sprite Icon  => icon;
    public int Cost => cost;
    public TileCode TileCode  => tileCode;
    public BattleResourceType ResourceType => resourceType;
}

public enum BattleResourceType : byte
{
    Unit = 0,
    Turret = 1,
    Wall = 2
}

[Serializable]
public class Stat
{
    [SerializeField]
    private string name = string.Empty;
    [SerializeField]
    private int points = 0;

    public int Points => points;
    public string Name => name;

    public override string ToString()
    {
        return $"{name}: {points}";
    }
}
