using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallController : MonoBehaviour, IDestroyable
{
    public string ForPlayer { get; set; }
    bool unRegistered = false;

    public GameObject GameObject => gameObject;

    public void OnDestroyed_EventListener()
    {
        if (unRegistered) return;
        unRegistered = true;

        PathFindManager.Instance.UnregisterPlayerResource(ForPlayer, transform, BattleResourceType.Wall);
    }
}
