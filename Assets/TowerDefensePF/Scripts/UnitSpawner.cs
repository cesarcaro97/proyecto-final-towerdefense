using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    public int unitSpawnTime = 0;
    public string forPlayer = string.Empty;
    public string enemyTag = string.Empty;
    public GameObject prefab = null;

    private bool spawnOnStart = true;
    public string unitTag = string.Empty;
    public int unitsLayer;
    internal GameObject unitIndicatorPrefab;


    public void OnUnitDestroyed(UnitController unitController)
    {
        print($"{forPlayer} unit destroyed");
        PathFindManager.Instance.UnregisterPlayerResource(forPlayer, unitController.transform, BattleResourceType.Unit);
        TriggerSpawn();
    }


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
        //uc.SetLayer(unitsLayer);
        uc.mySpanwer = this;
        uc.tag = unitTag;
        uc.player = forPlayer;
        uc.enemyTag = enemyTag;
        Instantiate(unitIndicatorPrefab, uc.transform);
        PathFindManager.Instance.RegistePlayerResource(forPlayer, uc.transform, BattleResourceType.Unit);
    }
}
