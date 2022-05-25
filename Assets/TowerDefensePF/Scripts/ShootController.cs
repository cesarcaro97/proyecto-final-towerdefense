using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] BulletController bulletPrefab = null;
    [SerializeField] float coolDownTime = 2;
    [SerializeField] float autoDestructTime = 1;

    public string enemyTag = string.Empty;
    float lastShot = 0;

    private void Start()
    {
        lastShot = Time.time - coolDownTime;
    }

    public void TryShoot(Vector2 targetPos, Action OnTargetKilled = null)
    {
        if (Time.time - lastShot >= coolDownTime)
        {
            lastShot = Time.time;
            Shoot(targetPos);
        }

    }

    private void Shoot(Vector3 targetPos)
    {
        Vector3 targetDir = targetPos - transform.position;
        var targetRot = Quaternion.FromToRotation(Vector3.up, targetDir);

        BulletController b = Instantiate(bulletPrefab, transform.position, targetRot);
        b.enemyTag = enemyTag;
        b.damage = damage;
        b.SetTarget(targetPos, enemyTag);
        Destroy(b.gameObject, autoDestructTime);
    }
}
