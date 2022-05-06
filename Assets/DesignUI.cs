using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesignUI : MonoBehaviour
{
    public BattleResource[] resourceCards = null;

    [SerializeField] DesignTabsUI designTabs = null;

    private void Awake()
    {
        Init();
    }

    void Init()
    {
        foreach (var r in resourceCards)
        {
            designTabs.AddResource(r);
        }
    }
}
