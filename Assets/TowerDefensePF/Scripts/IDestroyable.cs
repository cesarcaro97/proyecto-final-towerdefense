using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestroyable
{
    public GameObject GameObject { get; }
    public string ForPlayer { get; set; }
    void OnDestroyed_EventListener();
}
