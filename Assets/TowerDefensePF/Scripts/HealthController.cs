using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] UnityEvent onDestroyed = null;
    public int lifePoints = 10;

    public bool IsAlive => lifePoints > 0;

    BulletController bullet = null;

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        print("Take damage: " + damage);
        lifePoints -= damage;

        if(lifePoints <= 0)
        {
            lifePoints = 0;
            print("Destroy");
            Destroy(gameObject);
            onDestroyed?.Invoke();
        }
    }
}
