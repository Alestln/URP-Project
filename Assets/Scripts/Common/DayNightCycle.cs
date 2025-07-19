using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private Light2D _sunLight;

    [SerializeField]
    private float _cycleDurationSeconds = 120f;

    [SerializeField]
    private Gradient _lightColorGradient;

    [SerializeField]
    private AnimationCurve _lightIntensityCurve;

    private float _currentTime = 0f;

    private void Awake()
    {
        if (_sunLight is null)
        {
            Debug.LogError("Sun Light is not assigned in the DayNightCycle script.");
            enabled = false;
        }
    }

    private void Update()
    {
        _currentTime += Time.deltaTime;

        _currentTime %= _cycleDurationSeconds;

        // ��������� �������� ����� �� 0 �� 1
        // 0.0f - ������ ���
        // 0.25f - �������
        // 0.5f - �������
        // 0.75f - �����
        // 1.0f - ����

        float cyclePercentage = _currentTime / _cycleDurationSeconds;

        UpdateLight(cyclePercentage);
    }

    private void UpdateLight(float percent)
    {
        _sunLight.color = _lightColorGradient.Evaluate(percent);
        _sunLight.intensity = _lightIntensityCurve.Evaluate(percent);
    }
}
