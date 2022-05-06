using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BattleResourceCardUI : MonoBehaviour
{
    [SerializeField] Image resourceIconImage = null;
    [SerializeField] TMP_Text resourceNameText = null;
    [SerializeField] TMP_Text resourceCostText = null;

    public BattleResource Resource { get; private set; }

    public void Setup(BattleResource resource)
    {
        Resource = resource;
        resourceNameText.text = resource.Name;
        resourceCostText.text = resource.Cost.ToString("0");
        resourceIconImage.sprite = resource.Icon;
    }
}
