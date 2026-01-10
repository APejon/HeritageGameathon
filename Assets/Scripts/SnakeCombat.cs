using System;
using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

public class SnakeCombat : MonoBehaviour
{
    [SerializeField] private float snakeSpawnDistance = 2f;
    [SerializeField] private float attackSpeed = 0.5f;

    [SerializeField] private SpriteRenderer fInputPrompt;
    [SerializeField] private CinemachineCamera isometricCamera;
    private int fCounter;
    private bool forwardsBackwards;
    private float initialOrthographicSize;
    private Vector3 initialSnakePosition;


    private bool isBeingAttacked;
    public Action OnSnakeBite;

    public Action OnSnakeKilled;
    private GameObject snake;


    private void Start()
    {
        initialOrthographicSize = Camera.main.orthographicSize;
    }

    private void Update()
    {
        if (!isBeingAttacked)
        {
            return;
        }

        if (forwardsBackwards)
        {
            snake.transform.position = Vector3.MoveTowards(snake.transform.position, transform.position,
                Time.deltaTime * attackSpeed * Random.Range(0, 5));
            if (Vector3.Distance(snake.transform.position, transform.position) < 0.1f)
            {
                GetComponent<CinemachineImpulseSource>().GenerateImpulse();
                OnSnakeBite?.Invoke();
                forwardsBackwards = false;
            }
        }
        else
        {
            snake.transform.position = Vector3.MoveTowards(snake.transform.position, initialSnakePosition,
                Time.deltaTime * attackSpeed * Random.Range(0, 5));
            if (Vector3.Distance(snake.transform.position, initialSnakePosition) < 0.1f)
            {
                forwardsBackwards = true;
            }
        }

        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            fCounter++;
            snake.transform.DOShakeRotation(0.2f, 20, 5, 20);

            fInputPrompt.DOColor(new Color(0x75 / 255f, 1, 0xEC / 255f), 0.02f).SetLoops(2, LoopType.Yoyo);
            fInputPrompt.transform.DOScale(0.2f, 0.03f).SetRelative().SetLoops(2, LoopType.Yoyo);
            if (fCounter >= 15)
            {
                fInputPrompt.DOFade(0, 0.2f);
                snake.AddComponent<Rigidbody>().AddExplosionForce(1000, transform.position, 2f);
                isBeingAttacked = false;
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    Destroy(snake);
                    OnSnakeKilled?.Invoke();
                    DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                        value => isometricCamera.Lens.OrthographicSize = value, initialOrthographicSize, 0.2f);
                    fCounter = 0;
                });
            }
        }
    }


    public bool SpawnSnakeRandomly()
    {
        if (Random.Range(0, 100) < 30)
        {
            var insideUnitCircle = Random.insideUnitCircle.normalized;
            initialSnakePosition = transform.position +
                                   new Vector3(insideUnitCircle.x, 0, insideUnitCircle.y) * snakeSpawnDistance +
                                   Vector3.up * 0.2f;
            snake = Instantiate(Resources.Load<GameObject>("Prefabs/Snek"), initialSnakePosition, Quaternion.identity);
            snake.transform.LookAt(transform);
            isBeingAttacked = true;
            forwardsBackwards = true;
            // Camera.main.DOOrthoSize(1, 0.3f);
            fInputPrompt.color = Color.white;
            fInputPrompt.DOFade(1, 0.2f);
            DOTween.To(() => isometricCamera.Lens.OrthographicSize,
                value => isometricCamera.Lens.OrthographicSize = value, 1.68f, 0.2f);
            return true;
        }

        return false;
    }
}
