using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [Header("Настройки отображения")]
    [SerializeField] private Color _pathColor = Color.green;
    [SerializeField] private float _pointRadius = 0.3f;
    [SerializeField][Range(0f, 2f)] private float _labelOffsetY = 0.5f;

    [Header("Настройки стрелок")]
    [SerializeField] private float _arrowSize = 0.4f;
    [SerializeField][Range(10f, 45f)] private float _arrowAngle = 25f;

    public int Length => transform.childCount;

    public Transform GetPointTransform(int index)
    {
        Transform child = transform.GetChild(index);

        if (Length <= index || child is null)
        {
            return transform;
        }

        return child;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (Length == 0)
        {
            return;
        }

        for (int i = 0; i < Length; i++)
        {
            Vector3 currentPoint = GetPointTransform(i).position;

            Gizmos.color = _pathColor;
            Gizmos.DrawWireSphere(currentPoint, _pointRadius);

            UnityEditor.Handles.color = _pathColor;
            string pointLabel = i.ToString();
            Vector3 labelPosition = currentPoint + (Vector3.up * _labelOffsetY);
            UnityEditor.Handles.Label(labelPosition, pointLabel);

            if (Length > 1)
            {
                Vector3 nextPoint = GetPointTransform((i + 1) % Length).position;
                DrawArrow(currentPoint, nextPoint);
            }
        }
    }

    private void DrawArrow(Vector3 start, Vector3 end)
    {
        UnityEditor.Handles.color = _pathColor;
        UnityEditor.Handles.DrawLine(start, end);

        Vector3 direction = (end - start).normalized;
        if (direction == Vector3.zero)
        {
            return;
        }

        Vector3 rightWing = Quaternion.Euler(0, 0, _arrowAngle) * (-direction);
        Vector3 leftWing = Quaternion.Euler(0, 0, -_arrowAngle) * (-direction);

        UnityEditor.Handles.DrawLine(end, end + rightWing * _arrowSize);
        UnityEditor.Handles.DrawLine(end, end + leftWing * _arrowSize);
    }
#endif
}
