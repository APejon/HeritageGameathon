using System;
using DG.Tweening;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public static DayNightCycle Instance;
    [SerializeField] private int stepsInADay = 25;
    [SerializeField] private CharacterMovement characterMovement;
    [SerializeField] private float dayTemperature;
    [SerializeField] private float nightTemperature;
    [SerializeField] private Light sunLight1;
    [SerializeField] private Light sunLight2;
    [SerializeField] private Vector2 sunLight1DayNightIntensity;
    [SerializeField] private Vector2 sunLight2DayNightIntensity;
    [SerializeField] private Color characterSpriteNightColor;
    [SerializeField] private SpriteRenderer characterSpriteRenderer;
    private int accumulatedSteps;
    public Action OnDayStart;

    public Action OnNightStart;


    public bool IsNight => accumulatedSteps % stepsInADay >= stepsInADay / 2;
    public bool IsDay => !IsNight;


    private void Awake()
    {
        Instance = this;
        characterMovement.onMove += OnMove;
    }

    private void OnDestroy()
    {
        characterMovement.onMove -= OnMove;
    }

    private void OnMove()
    {
        var wasNight = IsNight;
        accumulatedSteps++;
        var t = Mathf.PingPong(accumulatedSteps / (float)stepsInADay, 1);
        DOTween.To(() => sunLight1.colorTemperature, x => sunLight1.colorTemperature = x,
            Mathf.Lerp(dayTemperature, nightTemperature, t), 0.5f);
        DOTween.To(() => sunLight2.colorTemperature, x => sunLight2.colorTemperature = x,
            Mathf.Lerp(dayTemperature, nightTemperature, t), 0.5f);
        DOTween.To(() => sunLight1.intensity, x => sunLight1.intensity = x,
            Mathf.Lerp(sunLight1DayNightIntensity.x, sunLight1DayNightIntensity.y, t), 0.5f);
        DOTween.To(() => sunLight2.intensity, x => sunLight2.intensity = x,
            Mathf.Lerp(sunLight2DayNightIntensity.x, sunLight2DayNightIntensity.y, t), 0.5f);
        characterSpriteRenderer.DOColor(Color.Lerp(Color.white, characterSpriteNightColor, t), 0.5f);
        if (wasNight && !IsNight)
        {
            OnDayStart?.Invoke();
        }
        else if (!wasNight && IsNight)
        {
            OnNightStart?.Invoke();
        }
    }

    public void MakeDay()
    {
        characterMovement.onMove -= OnMove;
        DOTween.To(() => sunLight1.colorTemperature, x => sunLight1.colorTemperature = x,
            Mathf.Lerp(dayTemperature, nightTemperature, 0), 2);
        DOTween.To(() => sunLight2.colorTemperature, x => sunLight2.colorTemperature = x,
            Mathf.Lerp(dayTemperature, nightTemperature, 0), 2);
        DOTween.To(() => sunLight1.intensity, x => sunLight1.intensity = x,
            Mathf.Lerp(sunLight1DayNightIntensity.x, sunLight1DayNightIntensity.y, 0), 2);
        DOTween.To(() => sunLight2.intensity, x => sunLight2.intensity = x,
            Mathf.Lerp(sunLight2DayNightIntensity.x, sunLight2DayNightIntensity.y, 0), 2);
        characterSpriteRenderer.DOColor(Color.Lerp(Color.white, characterSpriteNightColor, 0), 2);
    }
}
