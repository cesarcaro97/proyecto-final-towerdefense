using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private Transform target = null;
    public int damage = 0;

    Vector3 targetPos;

    private void Update()
    {
        if(target)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root != target) return;

        HealthController health = collision.transform.root.GetComponent<HealthController>();
        health.TakeDamage(damage);

        Destroy(gameObject);
    }

    public void SetTarget(Transform target)
    {
        targetPos = target.position;
        this.target = target;
    }
}
