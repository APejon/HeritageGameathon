using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class QuickSandInteraction : MonoBehaviour
{
    [SerializeField] private float drownSpeed = 0.5f;

    [SerializeField] private SpriteRenderer fInputPrompt;
    [SerializeField] private CinemachineCamera isometricCamera;
    [SerializeField] private float drownThreshold = 1;
    [SerializeField] private float floatAmount = 1;
    private float drownAmount;
    private float initialOrthographicSize;


    private bool isDrowning;

    public Action OnEscapedQuicksand;
    public Action OnTakenDamage;
    private float playerInitialYPosition;


    private void Start()
    {
        initialOrthographicSize = 3.28f;
        drownAmount = 0f;
        FindAnyObjectByType<ResourceBars>().death += OnDeath;
    }

    private void Update()
    {
        if (!isDrowning)
        {
            return;
        }

        if (drownAmount > drownThreshold)
        {
            OnTakenDamage?.Invoke();
            MessagesConcept.instance.SetText("You're drowning! 10 health lost!");
            drownAmount = 0;
        }

        // transform.Translate(Vector3.down * (drownSpeed * Time.deltaTime));


        drownAmount += Time.deltaTime;

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            fInputPrompt.DOColor(new Color(0x75 / 255f, 1, 0xEC / 255f), 0.02f).SetLoops(2, LoopType.Yoyo);
            fInputPrompt.transform.DOScale(0.2f, 0.03f).SetRelative().SetLoops(2, LoopType.Yoyo);
            fInputPrompt.DOFade(0, 0.2f);
            var position = transform.position;
            position.y += floatAmount;
            transform.position = position;
            if (transform.position.y >= playerInitialYPosition)
            {
                position = transform.position;
                position.y = playerInitialYPosition;
                transform.position = position;
                isDrowning = false;
                OnEscapedQuicksand?.Invoke();
                DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                    value => isometricCamera.Lens.OrthographicSize = value, initialOrthographicSize, 0.2f);
            }
        }
        else
        {
            var position = transform.position;
            position.y -= drownSpeed * Time.deltaTime;
            if (position.y < -0.5)
            {
                position.y = -0.5f;
            }

            transform.position = position;
        }
    }

    private void OnDeath()
    {
        FindAnyObjectByType<ResourceBars>().death -= OnDeath;
        enabled = false;
    }


    public void StartDrowning()
    {
        AudioManager.instance.playSFX(AudioManager.soundEffect.QUICKSAND, true);
        playerInitialYPosition = transform.position.y;
        isDrowning = true;
        fInputPrompt.color = Color.white;
        fInputPrompt.DOFade(1, 0.2f);
        DOTween.To(() => isometricCamera.Lens.OrthographicSize,
            value => isometricCamera.Lens.OrthographicSize = value, 1.68f, 0.2f);
        MessagesConcept.instance.SetText("Quicksand! get out quickly!");
    }
}
