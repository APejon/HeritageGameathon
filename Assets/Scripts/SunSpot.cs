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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                value => isometricCamera.Lens.OrthographicSize = value, 1.68f, 0.2f);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time = 0;
            DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                value => isometricCamera.Lens.OrthographicSize = value, initialOrthographicSize, 0.2f);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            time += Time.deltaTime;
            if (time >= 0.4f)
            {
                time = 0;
                resourceBars.decreaseResource(ResourceBars.stat.Hydration, 10);
            }
        }
    }
}
