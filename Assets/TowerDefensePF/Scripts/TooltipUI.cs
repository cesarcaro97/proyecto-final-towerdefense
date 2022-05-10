using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance { get; private set; }

    [SerializeField] Image panel = null;
    [SerializeField] TMP_Text text = null;

    public bool IsOpen => panel.isActiveAndEnabled;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Show(default, false);
    }

    private void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    public void Show(Vector2 pos, bool show, string content = null)
    {
        if (IsOpen == show) return;
        
        if (show)
            panel.transform.position = pos;

        if (!string.IsNullOrEmpty(content))
            text.text = content;

        panel.gameObject.SetActive(show);
    }
}
