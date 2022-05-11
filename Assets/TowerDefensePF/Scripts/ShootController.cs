using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    [SerializeField] int damage = 10;
    [SerializeField] BulletController bulletPrefab = null;
    [SerializeField] float coolDownTime = 2;

    float lastShot = 0;

    private void Start()
    {
        lastShot = Time.time - coolDownTime;
    }

    public void TryShoot(Transform target)
    {
        if (Time.time - lastShot >= coolDownTime)
        {
            lastShot = Time.time;
            Shoot(target);
        }
    }

    private void Shoot(Transform target)
    {
        Vector2 targetDir = target.position - transform.position;
        var targetRot = Quaternion.FromToRotation(Vector3.up, targetDir);

        BulletController b = Instantiate(bulletPrefab, transform.position, targetRot);
        b.damage = damage;
        b.SetTarget(target);
        Destroy(b.gameObject, 1);
    }
}
