using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    //private Transform target = null;
    public int damage = 0;
    public string enemyTag = string.Empty;

    Vector3 targetPos;
    bool hasTargetSet = false;

    private void Update()
    {
        if (hasTargetSet)
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * 5);
    }

    public void SetTarget(Vector2 targetPos, string enemyTag)
    {
        this.targetPos = targetPos;
        hasTargetSet = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var o = collision.transform.root;
        print(o.name);
        if (o.tag != enemyTag) return;

        var h = o.GetComponent<HealthController>();

        if (h == null) return;

        h.TakeDamage(damage);
        Destroy(gameObject);
    }
}
