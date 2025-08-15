using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [Header("Patrol points")]
    [SerializeField] private List<PatrolPoint> _points = new List<PatrolPoint>();

    [Header("Visualization")]
    [SerializeField] private Color _pathColor = Color.green;
    [SerializeField][Range(0f, 2f)] private float _labelOffsetY = 0.5f;

    [Header("Arrow settings")]
    [SerializeField] private float _arrowSize = 0.4f;
    [SerializeField][Range(10f, 45f)] private float _arrowAngle = 25f;

    public int Length => _points.Count;

    public PatrolPoint GetPoint(int index)
    {
        if (index < 0 || index >= _points.Count)
        {
            Debug.LogError($"[{nameof(PatrolPath)}] Неверный индекс точки: {index}. Всего точек: {Length}");
            throw new System.ArgumentOutOfRangeException($"[{nameof(PatrolPath)}] Неверный индекс точки: {index}. Всего точек: {Length}");
        }

        return _points[index];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _points.Clear();
        foreach (Transform child in transform)
        {
            PatrolPoint point = child.GetComponent<PatrolPoint>();
            if (point is not null)
            {
                _points.Add(point);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_points == null || _points.Count == 0)
            return;

        Handles.color = _pathColor;

        for (int i = 0; i < _points.Count; i++)
        {
            if (_points[i] == null) continue;

            Vector3 pos = _points[i].transform.position;

            // Порядковый номер под точкой
            Vector3 labelPos = pos - Vector3.up * _labelOffsetY;
            Handles.Label(labelPos, i.ToString());

            // Линия и стрелка к следующей точке
            Vector3 nextPos = _points[(i + 1) % _points.Count].transform.position;
            Handles.DrawLine(pos, nextPos);

            // Стрелка направления
            DrawArrow(pos, nextPos);
        }
    }

    private void DrawArrow(Vector3 from, Vector3 to)
    {
        Vector3 direction = (to - from).normalized;
        Vector3 middle = Vector3.Lerp(from, to, 0.5f);

        // Основная линия стрелки
        Handles.DrawLine(middle, middle - direction * _arrowSize);

        // "Крылья" стрелки
        Vector3 right = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, _arrowAngle) * Vector3.up;
        Vector3 left = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, -_arrowAngle) * Vector3.up;

        Handles.DrawLine(middle, middle - right * _arrowSize);
        Handles.DrawLine(middle, middle - left * _arrowSize);
    }
#endif
}
