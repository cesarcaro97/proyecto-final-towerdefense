using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public string forPlayer = string.Empty;
    public string enemyTag = string.Empty;
    ShootController shootController = null;
    Transform target = null;
    List<Transform> inRange = new List<Transform>();

    private void Awake()
    {
        shootController = GetComponent<ShootController>();
    }

    private void Update()
    {
        if (target == null) return;

        Vector2 targetDir = target.position - transform.position;
        var targetRot = Quaternion.FromToRotation(Vector3.up, targetDir);
        transform.rotation = targetRot;
        shootController.TryShoot(target);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.root.tag != enemyTag) return;
        
        inRange.Add(collision.transform.root);
        
        if(target == null)
            target = collision.transform.root;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.root.tag != enemyTag) return;

        inRange.Remove(collision.transform.root);

        if (collision.transform.root != target) return;

        target = null;

        if (inRange.Count > 0)
            target = inRange[0];

    }

    public void UnregisterFromBattleMap()
    {
        var p = PathFindManager.Instance.turretsByPlayer[forPlayer].Where(p => p.x == (int)transform.position.x && p.y == (int)transform.position.y).FirstOrDefault();

        if (p != default)
        {
            PathFindManager.Instance.turretsByPlayer[forPlayer].Remove(p);
        }
    }
}
