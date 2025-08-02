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

    public Transform Target => _target;

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

        if (Vector2.Angle(transform.up, directionToTarget) > _viewAngle / 2)
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
}
