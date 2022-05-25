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
    [SerializeField] float shootDistance = 0.5f;

    bool unRegistered = false;

    private void Awake()
    {
        shootController = GetComponent<ShootController>();
    }

    private void Update()
    {
        inRange = Physics2D.OverlapCircleAll(transform.position, shootDistance).Select(x => x.transform).Where(x => x.tag == enemyTag).ToList();

        if(target == null)
        {
            if(inRange.Count > 0)
                target = inRange[0];
        }
        else
        {
            if(!inRange.Contains(target))
            {
                if(inRange.Count > 0)
                    target = inRange[0];
            }
        }

        if (target == null) return;

        Vector2 targetDir = target.position - transform.position;
        var targetRot = Quaternion.FromToRotation(Vector3.up, targetDir);
        transform.rotation = targetRot;
        shootController.enemyTag = enemyTag;
        shootController.TryShoot(target.position);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.root.tag != enemyTag) return;

    //    inRange.Add(collision.transform.root);

    //    if(target == null)
    //        target = collision.transform.root;
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.transform.root.tag != enemyTag) return;

    //    inRange.Remove(collision.transform.root);

    //    if (collision.transform.root != target) return;

    //    target = null;

    //    if (inRange.Count > 0)
    //        target = inRange[0];

    //}

    public void UnregisterFromBattleMap()
    {
        if (unRegistered) return;

        unRegistered = true;
        PathFindManager.Instance.UnregisterPlayerResource(forPlayer, transform, BattleResourceType.Turret);
    }
}
