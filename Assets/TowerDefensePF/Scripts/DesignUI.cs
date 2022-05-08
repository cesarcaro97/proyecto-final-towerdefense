using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesignUI : MonoBehaviour
{
    [SerializeField] DesignTabsUI designTabs = null;
    [SerializeField] TMP_Text budgetText = null;
    [SerializeField] TMP_Text playerText = null;
    

    public void Init(BattleResource[] battleResources)
    {
        foreach (var r in battleResources)
        {
            designTabs.AddResource(r);
        }
    }

    public void SetBudgetText(int budget)
    {
        budgetText.text = budget.ToString();
    }
    public void SetPlayerText(string player)
    {
        playerText.text = $"{player} Design";
    }

    public void OnReady_ButtonClick()
    {
        DesignManager.Instance.Ready();
    }
}
