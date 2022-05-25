using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    //[SerializeField] GameObject[]  = null;
    [SerializeField] TileCode unitType = TileCode.Unit_Soldier_A;
    [SerializeField] float speed = 1;
    [SerializeField] float shootDistance = 0.5f;
    public UnitSpawner mySpanwer = null;
    public string player = string.Empty;
    
    Vector2 nextPathPosition;
    public string enemyTag;

    private Vector2 shootPosition;
    bool shootPosIsSameTarget = false;
    ShootController shootController = null;

    public TileCode UnitType => unitType;
    private Transform Target { get; set; }
    private bool HasTarget => Target != null;


    private void Awake()
    {
        shootController = GetComponent<ShootController>();
    }

    private void Start()
    {
        Target = null;
        nextPathPosition = transform.position;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.transform.root.tag != enemyTag) return;

    //    inRange.Add(collision.transform.root);

    //    if (shootTarget == null)
    //        shootTarget = collision.transform.root;
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.transform.root.tag != enemyTag) return;

    //    inRange.Remove(collision.transform.root);

    //    if (collision.transform.root != shootTarget) return;

    //    shootTarget = null;

    //    if (inRange.Count > 0)
    //        shootTarget = inRange[0];

    //}

    private void Update()
    {
        if (HasTarget)
        {
            if (IsMoving())
                transform.position = Vector3.MoveTowards(transform.position, nextPathPosition, speed * Time.deltaTime);
            else
            {
                transform.position = nextPathPosition;
                nextPathPosition = transform.position;

                Vector2 destPos = shootPosIsSameTarget ? (Vector2)Target.position : shootPosition;
                Vector2 nextPoint = PathFindManager.Instance.GetFirstPathPoint(transform.position, new Vector2(destPos.x, destPos.y));
                if (nextPoint != Vector2.one * -1)
                    SetNextPositionPos(nextPoint);
                else
                    print("no move");
            }

            if (HasTarget)
            //if (shootPosition != Vector2.one * -1)
            {
                if (Vector2.Distance(transform.position, Target.position) < shootDistance)
                {
                    shootController.enemyTag = enemyTag;
                    shootController.TryShoot(Target.position);
                }
            }
        }
        else
        {
            TrySetTarget();
        }

        //if(shootTarget)
        //{
        //    shootController.enemyTag = enemyTag;
        //    shootController.TryShoot(shootTarget);
        //}

        //if (Input.GetMouseButtonDown(0))
        //{
        //    var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //    int x = Mathf.RoundToInt(p.x);
        //    int y = Mathf.RoundToInt(p.y);
            
        //    Vector2 nextPoint = PathFindManager.Instance.GetFirstPathPoint(transform.position , new Vector2(x, y));
        //    if (nextPoint != Vector2.one * -1)
        //        SetTargetPos(nextPoint);
        //    else
        //        print("no point" + nextPoint) ;
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, shootDistance);
    }

    private void TrySetTarget()
    {
        Transform target;
        switch (unitType)
        {
            case TileCode.Unit_Soldier_A:
                target = PathFindManager.Instance.GetTurretTarget(player);
                if(target == null)
                    target = PathFindManager.Instance.GetWallTarget(player);
                if (target == null)
                    target = PathFindManager.Instance.GetCoreTarget(player);

                if (target != null)
                {
                    Target = target;
                    shootPosition = PathFindManager.Instance.GetPointNextTo(target.position);
                }
                else
                {
                    shootPosition = Vector2.one * -1;
                }
                break;
            case TileCode.Unit_Soldier_D:

                int r = Random.Range(0, 2);
                if(r == 0)
                {
                    target = PathFindManager.Instance.GetUnitTarget(player, TileCode.Unit_Soldier_A);
                    if (target == null)
                        target = PathFindManager.Instance.GetUnitTarget(player, TileCode.Unit_Hero);                    
                }
                else
                {
                    target = PathFindManager.Instance.GetUnitTarget(player, TileCode.Unit_Hero);
                    if (target == null)
                        target = PathFindManager.Instance.GetUnitTarget(player, TileCode.Unit_Soldier_A);
                }

                if (target != null)
                {
                    Target = target;
                    shootPosIsSameTarget = true;
                }
                else
                {
                    shootPosition = Vector2.one * -1;
                }

                break;
            case TileCode.Unit_Hero:
                int rh = Random.Range(0, 2);
                if (rh == 0)
                {
                    target = PathFindManager.Instance.GetUnitTarget(player, TileCode.Unit_Soldier_D);
                    if (target == null)
                    {
                        target = PathFindManager.Instance.GetCoreTarget(player);
                        if(target != null)
                        {
                            shootPosIsSameTarget = false;
                            Target = target;
                            shootPosition = PathFindManager.Instance.GetPointNextTo(target.position);
                        }
                        else
                        {
                            shootPosition = Vector2.one * -1;
                        }
                    }
                    else
                    {
                        shootPosIsSameTarget = true;
                    }
                }
                else
                {
                    target = PathFindManager.Instance.GetCoreTarget(player);
                    if (target == null)
                    {
                        target = PathFindManager.Instance.GetUnitTarget(player, TileCode.Unit_Soldier_D);

                        if(target != null)
                        {
                            Target = target;
                            shootPosIsSameTarget = false;
                        }
                    }
                    else
                    {
                        shootPosIsSameTarget = true;
                    }
                }
                break;
            default:
                throw new System.ArgumentException("Unit type not valid");
        }

        
    }


    public void OnDestroyed_EventListener()
    {
        mySpanwer.OnUnitDestroyed(this);
    }

    public void SetNextPositionPos(Vector2 target)
    {
        nextPathPosition = target;
        transform.rotation = GetMovementRotation(GetMovementDirection(nextPathPosition));
    }

    private MovementDirection GetMovementDirection(Vector2 targetPosition)
    {
        if (targetPosition.x < transform.position.x)
            return MovementDirection.Left;
        else if (targetPosition.x > transform.position.x)
            return MovementDirection.Right;
        else if (targetPosition.y < transform.position.y)
            return MovementDirection.Down;
        else if (targetPosition.y > transform.position.y)
            return MovementDirection.Up;

        return MovementDirection.Idle;
    }

    private Quaternion GetMovementRotation(MovementDirection movementDirection)
    {
        switch (movementDirection)
        {
            case MovementDirection.Up:
                return Quaternion.AngleAxis(90, Vector3.forward);
            case MovementDirection.Right:
                return Quaternion.AngleAxis(0, Vector3.forward);
            case MovementDirection.Down:
                return Quaternion.AngleAxis(270, Vector3.forward);
            case MovementDirection.Left:
                return Quaternion.AngleAxis(180, Vector3.forward);
            default:
                return Quaternion.identity;
        }
    }

    public bool IsMoving()
    {
        return Vector3.Distance(transform.position, nextPathPosition) > 0.05f;
    }

    //internal bool HasTarget()
    //{
    //    return (Vector3)targetPosition != transform.position;
    //}
}
