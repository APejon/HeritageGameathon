using DG.Tweening;
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
    private Camera cam;
    private bool isMoving;
    private Vector3 targetPosition;
    private DownwardRaycast _raycastRef;


    private void Awake()
    {
        cam = Camera.main;
        _raycastRef = GetComponent<DownwardRaycast>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            Vector3 dir;
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                dir = Vector3.right - Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkRightSprite;
                targetPosition = transform.position + dir * 1.85f;
                dInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                wInputPrompt.DOFade(0, 0.05f);
                sInputPrompt.DOFade(0, 0.05f);
                aInputPrompt.DOFade(0, 0.05f);
            }
            else if (Keyboard.current.aKey.wasPressedThisFrame)
            {
                dir = -Vector3.right + Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkLeftSprite;
                targetPosition = transform.position + dir * 1.85f;
                aInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                wInputPrompt.DOFade(0, 0.05f);
                sInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
            }
            else if (Keyboard.current.wKey.wasPressedThisFrame)
            {
                dir = Vector3.right + Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkIdleSprite;
                targetPosition = transform.position + dir * 1.85f;
                wInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                aInputPrompt.DOFade(0, 0.05f);
                sInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
            }
            else if (Keyboard.current.sKey.wasPressedThisFrame)
            {
                dir = -Vector3.right - Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkIdleSprite;
                targetPosition = transform.position + dir * 1.85f;
                sInputPrompt.color = new Color(0x75 / 255f, 1, 0xEC / 255f);
                wInputPrompt.DOFade(0, 0.05f);
                aInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
            }
            else
            {
                CharacterSpriteRenderer.sprite = walkIdleSprite;
            }
            var tileEvent = PerformLookAhead(targetPosition);
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
                ResetInputPrompts();
                visibilityToggler.ToggleVisibility();
                _raycastRef.castARay();
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

    private EventCollision.events PerformLookAhead(Vector3 newPosition)
    {
        var raycast = Physics.Raycast(new Ray(newPosition, Vector3.down), out var hit, 100f);

        if (raycast)
        {
            var tileEvent = hit.transform.gameObject.GetComponent<EventCollision>();
            if (tileEvent == null) return EventCollision.events.None;
            return tileEvent._event;
        }
        return EventCollision.events.None;
    }

    private void ResetInputPrompts()
    {
        wInputPrompt.color = Color.white;
        aInputPrompt.color = Color.white;
        sInputPrompt.color = Color.white;
        dInputPrompt.color = Color.white;
    }
}
