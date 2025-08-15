using UnityEngine;

public class PatrolPoint : MonoBehaviour
{
    [Header("Visualization")]
    [SerializeField] private Color _pointColor = Color.yellow;
    [SerializeField] private float _radius = 0.3f;

    [Header("Wait Settings")]
    [SerializeField][Min(0f)] private float _waitTime = 0f;

    public Vector2 Position => transform.position;
    public float WaitTime => _waitTime;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = _pointColor;
        Gizmos.DrawSphere(transform.position, _radius);
    }
#endif
}
