using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private Sprite walkLeftSprite;
    [SerializeField] private Sprite walkRightSprite;
    [SerializeField] private Sprite walkIdleSprite;
    [SerializeField] private CharacterVisibilityToggler visibilityToggler;
    public SpriteRenderer CharacterSpriteRenderer;

    [SerializeField] private SpriteRenderer wInputPrompt;
    [SerializeField] private SpriteRenderer aInputPrompt;
    [SerializeField] private SpriteRenderer sInputPrompt;
    [SerializeField] private SpriteRenderer dInputPrompt;
    [SerializeField] private SnakeCombat snakeCombat;
    [SerializeField] private QuickSandInteraction quickSandInteraction;
    private DownwardRaycast _raycastRef;
    private ResourceBars _resourceRef;
    private Animator animator;
    private bool isMoving;
    public Action onMove;
    private TweenerCore<Vector3, Vector3, VectorOptions> snakeTween;
    private int steps;
    private Vector3 targetPosition;


    private void Awake()
    {
        _raycastRef = GetComponent<DownwardRaycast>();
        animator = GetComponent<Animator>();
        _resourceRef = GetComponent<ResourceBars>();
    }

    private void Start()
    {
        _resourceRef.death += OnDeath;
    }

    private void Update()
    {
        EventCollision.events tileEvent;
        if (!isMoving)
        {
            Vector3 dir;
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                dir = Vector3.right - Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkRightSprite;
                animator.SetTrigger("WalkR");
                targetPosition = transform.position + dir * 1.85f;
                dInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                wInputPrompt.DOFade(0, 0.05f);
                sInputPrompt.DOFade(0, 0.05f);
                aInputPrompt.DOFade(0, 0.05f);
                _raycastRef.setEvent();
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                dir = -Vector3.right + Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkLeftSprite;
                animator.SetTrigger("WalkL");
                targetPosition = transform.position + dir * 1.85f;
                aInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                wInputPrompt.DOFade(0, 0.05f);
                sInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
                _raycastRef.setEvent();
            }
            else if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                dir = Vector3.right + Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkRightSprite;
                animator.SetTrigger("WalkR");
                targetPosition = transform.position + dir * 1.85f;
                wInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                aInputPrompt.DOFade(0, 0.05f);
                sInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
                _raycastRef.setEvent();
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                dir = -Vector3.right - Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkLeftSprite;
                animator.SetTrigger("WalkL");
                targetPosition = transform.position + dir * 1.85f;
                sInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                wInputPrompt.DOFade(0, 0.05f);
                aInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
                _raycastRef.setEvent();
            }

            // CharacterSpriteRenderer.sprite = walkIdleSprite;
            tileEvent = RaycastForTileType(targetPosition);
            if (tileEvent == EventCollision.events.Boundary)
            {
                targetPosition = transform.position;
                DOVirtual.DelayedCall(0.1f, ResetInputPrompts);
            }
        }

        if (targetPosition == Vector3.zero || targetPosition == transform.position)
        {
            if (isMoving)
            {
                steps++;
                AudioManager.instance.randomizeFootPitch();
                AudioManager.instance.playFootstep(AudioManager.soundEffect.FOOTSTEP);
                if (steps % 10 == 0)
                {
                    _resourceRef.decreaseResource(ResourceBars.stat.Hunger, 10);
                    _resourceRef.decreaseResource(ResourceBars.stat.Hydration, 10);
                    MessagesConcept.instance.SetText("Time passed, hydration and hunger used.");
                }
                ResetInputPrompts();
                visibilityToggler.ToggleVisibility();
                _raycastRef.castARay();
                tileEvent = RaycastForTileType(targetPosition);
                if (tileEvent == EventCollision.events.PatchOfGrass)
                {
                    if (snakeCombat.SpawnSnakeRandomly())
                    {
                        AudioManager.instance.playSFX(AudioManager.soundEffect.SNAKE, true);
                        enabled = false;
                        snakeCombat.OnSnakeKilled += OnSnakeKilled;
                    }
                }
                else if (tileEvent == EventCollision.events.QuickSand)
                {
                    enabled = false;
                    quickSandInteraction.StartDrowning();
                    quickSandInteraction.OnEscapedQuicksand += OnEscapedQuicksand;
                }

                onMove?.Invoke();
            }

            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (isMoving)
        {
            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition, animationSpeed * Time.deltaTime);
        }
    }

    private void OnEnable()
    {
        wInputPrompt.DOFade(1, 0.1f);
        aInputPrompt.DOFade(1, 0.1f);
        sInputPrompt.DOFade(1, 0.1f);
        dInputPrompt.DOFade(1, 0.1f);
    }

    private void OnDisable()
    {
        wInputPrompt.DOFade(0, 0.1f);
        aInputPrompt.DOFade(0, 0.1f);
        sInputPrompt.DOFade(0, 0.1f);
        dInputPrompt.DOFade(0, 0.1f);
        CharacterSpriteRenderer.sprite = walkIdleSprite;
    }

    private void OnDeath()
    {
        enabled = false;
        _resourceRef.death -= OnDeath;
        CharacterSpriteRenderer.DOFade(0, 1f);
        wInputPrompt.gameObject.SetActive(false);
        aInputPrompt.gameObject.SetActive(false);
        sInputPrompt.gameObject.SetActive(false);
        dInputPrompt.gameObject.SetActive(false);
    }


    private void OnEscapedQuicksand()
    {
        quickSandInteraction.OnEscapedQuicksand -= OnEscapedQuicksand;
        AudioManager.instance.playSFX(AudioManager.soundEffect.QUICKSAND, false);
        enabled = true;
    }

    private void OnSnakeKilled()
    {
        snakeCombat.OnSnakeKilled -= OnSnakeKilled;
        AudioManager.instance.playSFX(AudioManager.soundEffect.SNAKE, false);
        enabled = true;
    }

    private static EventCollision.events RaycastForTileType(Vector3 newPosition)
    {
        var raycast = Physics.Raycast(new Ray(newPosition, Vector3.down), out var hit, 100f,
            1 << LayerMask.NameToLayer("Tile"));

        if (raycast)
        {
            var tileEvent = hit.transform.gameObject.GetComponent<EventCollision>();
            if (tileEvent == null)
            {
                return EventCollision.events.None;
            }

            return tileEvent._event;
        }

        return EventCollision.events.None;
    }

    private static EventCollision.trackType RaycastForTrackType(Vector3 newPosition)
    {
        var raycast = Physics.Raycast(new Ray(newPosition, Vector3.down), out var hit, 100f,
            1 << LayerMask.NameToLayer("Tile"));

        if (raycast)
        {
            var tileEvent = hit.transform.gameObject.GetComponent<EventCollision>();
            if (tileEvent == null)
            {
                return EventCollision.trackType.Camel;
            }

            return tileEvent._track;
        }

        return EventCollision.trackType.Camel;
    }

    private void ResetInputPrompts()
    {
        wInputPrompt.color = Color.white;
        aInputPrompt.color = Color.white;
        sInputPrompt.color = Color.white;
        dInputPrompt.color = Color.white;
    }
}
