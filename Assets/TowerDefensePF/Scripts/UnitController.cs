using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour
{
    [SerializeField] float speed = 1;
    public UnitSpawner mySpanwer = null;

    Vector2 targetPosition;


    private void Start()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        if (HasTarget())
        {
            if (IsMoving())
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            else
            {
                transform.position = targetPosition;
                targetPosition = transform.position;
            }
        }
        else
        {

        }

        if (Input.GetMouseButtonDown(0))
        {
            var p = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            int x = Mathf.RoundToInt(p.x);
            int y = Mathf.RoundToInt(p.y);
            
            Vector2 nextPoint = PathFindManager.Instance.GetFirstPathPoint(transform.position , new Vector2(x, y));
            if (nextPoint != default)
                SetTargetPos(nextPoint);
            else
                print("default point");
        }
    }


    public void OnDestroyed_EventListener()
    {
        mySpanwer.OnUnitDestroyed(this);
    }

    public void SetTargetPos(Vector2 target)
    {
        targetPosition = target;
        transform.rotation = GetMovementRotation(GetMovementDirection(targetPosition));
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
        return Vector3.Distance(transform.position, targetPosition) > 0.05f;
    }

    internal bool HasTarget()
    {
        return (Vector3)targetPosition != transform.position;
    }
}
