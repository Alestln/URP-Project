using System;
using UnityEngine;
using UnityEngine.Events;
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

    [SerializeField]
    private float _currentTime = 0f;

    [Header("Time Thresholds")]
    [SerializeField][Range(0, 1)] private float _dayStartThreshold = 0.25f; // 6 AM
    [SerializeField][Range(0, 1)] private float _nightStartThreshold = 0.75f; // 6 PM

    [Header("Events")]
    public UnityEvent OnDayBegan;
    public UnityEvent OnNightBegan;

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

        // Вычисляем прогресс цикла от 0 до 1
        // 0.0f - начало дня
        // 0.25f - рассвет
        // 0.5f - полдень
        // 0.75f - закат
        // 1.0f - ночь

        float cyclePercentage = _currentTime / _cycleDurationSeconds;

        UpdateLight(cyclePercentage);
        CheckTimeState(cyclePercentage);
    }

    private void CheckTimeState(float percent)
    {
        if (percent >= _nightStartThreshold || percent < _dayStartThreshold)
        {
            OnNightBegan?.Invoke();
        }
        else if (percent >= _dayStartThreshold && percent < _nightStartThreshold)
        {
            OnDayBegan?.Invoke();
        }
    }

    private void UpdateLight(float percent)
    {
        _sunLight.color = _lightColorGradient.Evaluate(percent);
        _sunLight.intensity = _lightIntensityCurve.Evaluate(percent);
    }
}
