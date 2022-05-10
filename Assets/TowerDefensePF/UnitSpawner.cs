using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] float spawnTime = 15;

    public GameObject prefab = null;

    private bool spawnOnStart = true;

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
        yield return new WaitForSeconds(spawnTime);
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
    }
}
