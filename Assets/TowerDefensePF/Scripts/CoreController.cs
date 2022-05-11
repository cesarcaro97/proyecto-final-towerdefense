using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoreController : MonoBehaviour, IDestroyable
{
    public string ForPlayer { get; set; }

    public GameObject GameObject => gameObject;

    public void OnDestroyed_EventListener()
    {
        BattleManager.Instance.GameOver(ForPlayer);
    }
}
