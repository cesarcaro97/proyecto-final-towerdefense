using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public int unitSpawnTime = 0;
    public string forPlayer = string.Empty;
    public GameObject prefab = null;

    public void OnUnitDestroyed(UnitController unitController)
    {
        print($"{forPlayer} unit destroyed");
        PathFindManager.Instance.UnRegisterPlayerUnit(forPlayer, unitController.transform);
        TriggerSpawn();
    }

    private bool spawnOnStart = true;
    internal string unitTag = string.Empty;

    private void Start()
    {
        if (spawnOnStart)
            Spawn();
    }

    public void TriggerSpawn()
    {
        StartCoroutine(ISpawn());
    }

    IEnumerator ISpawn()
    {
        yield return new WaitForSeconds(unitSpawnTime);
        Spawn();
    }

    private void Spawn()
    {
        UnitController uc = Instantiate(prefab, transform.position, Quaternion.identity).GetComponent<UnitController>();
        uc.mySpanwer = this;
        uc.tag = unitTag;
        PathFindManager.Instance.RegisterPlayerUnit(forPlayer, uc.transform);
    }
}
