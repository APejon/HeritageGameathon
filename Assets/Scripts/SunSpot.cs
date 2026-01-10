using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class SunSpot : MonoBehaviour
{
    [SerializeField] private ResourceBars resourceBars;
    [SerializeField] private CinemachineCamera isometricCamera;
    private float initialOrthographicSize;

    private float time;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
        initialOrthographicSize = Camera.main.orthographicSize;
        DayNightCycle.Instance.OnNightStart += OnNightStart;
        DayNightCycle.Instance.OnDayStart += OnDayStart;
    }

    private void OnDestroy()
    {
        DayNightCycle.Instance.OnNightStart -= OnNightStart;
        DayNightCycle.Instance.OnDayStart -= OnDayStart;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                value => isometricCamera.Lens.OrthographicSize = value, 1.68f, 0.2f);
            AudioManager.instance.playSFX(AudioManager.soundEffect.BURNING, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = 0;
            DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                value => isometricCamera.Lens.OrthographicSize = value, initialOrthographicSize, 0.2f);
            AudioManager.instance.playSFX(AudioManager.soundEffect.BURNING, false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time += Time.deltaTime;
            if (time >= 1f)
            {
                time = 0;
                resourceBars.decreaseResource(ResourceBars.stat.Hydration, 10);
            }
        }
    }

    private void OnDayStart()
    {
        gameObject.SetActive(true);
    }

    private void OnNightStart()
    {
        gameObject.SetActive(false);
    }
}
