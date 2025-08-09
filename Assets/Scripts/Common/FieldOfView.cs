using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Параметры поля зрения")]
    [SerializeField] private float _viewRadius = 5f;

    [Range(0, 360)]
    [SerializeField] private float _viewAngle = 90f;

    [Header("Параметры обнаружения")]
    [SerializeField] private Transform _target;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Vector2 _forwardDirection = Vector2.up;

    public Transform Target => _target;

    public void SetDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _forwardDirection = direction.normalized;
        }
    }

    public bool IsTargetVisible()
    {
        if (_target is null)
        {
            return false;
        }

        float distanceToTarget = Vector2.Distance(transform.position, _target.position);

        if (distanceToTarget > _viewRadius)
        {
            return false;
        }

        Vector2 directionToTarget = (_target.position - transform.position).normalized;

        if (Vector2.Angle(_forwardDirection, directionToTarget) > _viewAngle / 2)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleLayerMask);

        if (hit.collider is not null)
        {
            return false;
        }

        return true;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.forward, _viewRadius);

        Vector3 viewAngleA = DirectionFromAngle(-_viewAngle / 2, _forwardDirection);
        Vector3 viewAngleB = DirectionFromAngle(_viewAngle / 2, _forwardDirection);

        Gizmos.DrawLine(transform.position, transform.position + viewAngleA * _viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + viewAngleB * _viewRadius);

        if (IsTargetVisible())
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }

    private Vector3 DirectionFromAngle(float angleInDegrees, Vector2 forwardDirection)
    {
        float baseAngle = Mathf.Atan2(forwardDirection.y, forwardDirection.x) * Mathf.Rad2Deg;
        float totalAngle = baseAngle + angleInDegrees;

        return new Vector3(Mathf.Cos(totalAngle * Mathf.Deg2Rad), Mathf.Sin(totalAngle * Mathf.Deg2Rad));
    }
#endif
}
