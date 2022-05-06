using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Battle Resource", menuName = "Battle Resource", order = 0)]
public class BattleResource : ScriptableObject
{
    [SerializeField]
    private string _name = string.Empty;
    [SerializeField]
    private Sprite icon = null;
    [SerializeField]
    private int cost = 0;
    [SerializeField]
    private BattleResourceType resourceType = BattleResourceType.Unit;
    [SerializeField]
    private TileCode tileCode = TileCode.Free;

    public string Name  => name; 
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

public enum TileCode
{
    Free = 0,
    Unit_Solider_A = 1,
    Unit_Soldier_D = 2,
    Unit_Hero = 3,
    Turret_T1 = 4,
    Turret_T2 = 5,
    Wall_Rock = 6,
    Wall_Concrete = 7,
}
