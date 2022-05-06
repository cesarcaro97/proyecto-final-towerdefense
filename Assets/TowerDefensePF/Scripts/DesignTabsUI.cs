using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DesignTabsUI : MonoBehaviour
{
    public const string UNITS_TAB_NAME = "units";
    public const string TURRETS_TAB_NAME = "turrets";
    public const string WALLS_TAB_NAME = "walls";

    [SerializeField] BattleResourceCardUI resourceCardPrefab = null;
    [SerializeField] Button unitsBtn = null;
    [SerializeField] Transform unitsContentPanel = null;
    [SerializeField] Button turretsBtn = null;
    [SerializeField] Transform turretsContentPanel = null;
    [SerializeField] Button wallsBtn = null;
    [SerializeField] Transform wallsContentPanel = null;

    private Dictionary<string, Transform> tabsContentPanelByName = null;
    private Dictionary<string, Button> tabsBtnByName = null;
    private string SelectedTab { get; set; }


    private void Awake()
    {
        Init();
    }
    private void Init()
    {
        tabsContentPanelByName = new Dictionary<string, Transform>()
        {
            { UNITS_TAB_NAME, unitsContentPanel },
            { TURRETS_TAB_NAME, turretsContentPanel },
            { WALLS_TAB_NAME, wallsContentPanel }
        };
        tabsBtnByName = new Dictionary<string, Button>()
        {
            { UNITS_TAB_NAME, unitsBtn },
            { TURRETS_TAB_NAME, turretsBtn },
            { WALLS_TAB_NAME, wallsBtn }
        };
        ShowTab(UNITS_TAB_NAME, false);
        ShowTab(TURRETS_TAB_NAME, false);
        ShowTab(WALLS_TAB_NAME, false);
        SelectTab(UNITS_TAB_NAME);
    }


    public void AddResource(BattleResource resource)
    {
        Transform panelParent = null;
        
        switch (resource.ResourceType)
        {
            case BattleResourceType.Unit:
                panelParent = tabsContentPanelByName[UNITS_TAB_NAME];
                break;
            case BattleResourceType.Turret:
                panelParent = tabsContentPanelByName[TURRETS_TAB_NAME];
                break;
            case BattleResourceType.Wall:
                panelParent = tabsContentPanelByName[WALLS_TAB_NAME];
                break;
            default:
                Debug.LogError("Not Implemented");
                break;
        }
        BattleResourceCardUI card = Instantiate(resourceCardPrefab, panelParent);
        card.Setup(resource);
    }

    public void SelectTab(string tabName)
    {
        EventSystem.current.SetSelectedGameObject(null);
        if (SelectedTab == tabName) return;

        if (!string.IsNullOrEmpty(SelectedTab))
        {
            ShowTab(SelectedTab, false);
        }

        SelectedTab = tabName;
        ShowTab(tabName, true);
    }

    private void ShowTab(string tabName, bool show)
    {
        tabsContentPanelByName[tabName].parent.gameObject.SetActive(show);
    }

}
