using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [Header("Параметры поля зрения")]
    [SerializeField] private float _viewRadius = 5f;

    [Range(0, 360)]
    [SerializeField] private float _viewAngle = 90f;

    [Header("Параметры обнаружения")]
    [SerializeField] private LayerMask _targetLayerMask;
    [SerializeField] private LayerMask _obstacleLayerMask;

    private Vector2 _forwardDirection = Vector2.up;

    private List<Transform> _visibleTargets = new List<Transform>();

    public void SetDirection(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            _forwardDirection = direction.normalized;
        }
    }

    public void FindVisibleTargets(List<Transform> visibleTargets)
    {
        visibleTargets.Clear();
        _visibleTargets.Clear();

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, _viewRadius, _targetLayerMask);

        foreach (var targetCollider in targetsInViewRadius)
        {
            Transform target = targetCollider.transform;
            Vector2 directionToTarget = (target.position - transform.position).normalized;

            if (Vector2.Angle(_forwardDirection, directionToTarget) < _viewAngle / 2)
            {
                float distanceToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, directionToTarget, distanceToTarget, _obstacleLayerMask))
                {
                    visibleTargets.Add(target);
                    _visibleTargets.Add(target); // Добавляем видимую цель в список
                }
            }
        }
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

        Gizmos.color = Color.red;
        foreach (var target in _visibleTargets)
        {
            if (target != null)
            {
                Gizmos.DrawLine(transform.position, target.position);
            }
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
