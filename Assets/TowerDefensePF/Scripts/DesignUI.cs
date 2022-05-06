using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DesignUI : MonoBehaviour
{
    [SerializeField] DesignTabsUI designTabs = null;
    [SerializeField] TMP_Text budgetText = null;

    public void Init(int budget, BattleResource[] battleResources)
    {
        SetBudgetText(budget);
        foreach (var r in battleResources)
        {
            designTabs.AddResource(r);
        }
    }

    public void SetBudgetText(int budget)
    {
        budgetText.text = budget.ToString();
    }
}
