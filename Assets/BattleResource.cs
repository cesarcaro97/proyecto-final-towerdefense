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

    public string Name { get => name; set => name = value; }
    public Sprite Icon { get => icon; set => icon = value; }
    public int Cost { get => cost; set => cost = value; }
    public BattleResourceType ResourceType { get => resourceType; set => resourceType = value; }
}

public enum BattleResourceType : byte
{
    Unit = 254,
    Turret = 253,
    Wall = 252
}
