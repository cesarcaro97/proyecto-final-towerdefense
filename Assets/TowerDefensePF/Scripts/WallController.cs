using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WallController : MonoBehaviour, IDestroyable
{
    
    public string ForPlayer { get; set; }

    public GameObject GameObject => gameObject;

    public void OnDestroyed_EventListener()
    {
        var p = PathFindManager.Instance.wallsByPlayer[ForPlayer].Where(p => p.x == (int)transform.position.x && p.y == (int)transform.position.y).FirstOrDefault();

        if (p != default)
        {
            PathFindManager.Instance.wallsByPlayer[ForPlayer].Remove(p);
        }
    }
}
