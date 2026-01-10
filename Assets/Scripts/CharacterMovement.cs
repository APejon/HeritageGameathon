using System;
using DG.Tweening;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float animationSpeed = 1f;
    [SerializeField] private Sprite walkLeftSprite;
    [SerializeField] private Sprite walkRightSprite;
    [SerializeField] private Sprite walkIdleSprite;
    public SpriteRenderer CharacterSpriteRenderer;
    private Camera cam;
    private Vector3 targetPosition;
    private bool isMoving;

    [SerializeField] private SpriteRenderer wInputPrompt;
    [SerializeField] private SpriteRenderer aInputPrompt;
    [SerializeField] private SpriteRenderer sInputPrompt;
    [SerializeField] private SpriteRenderer dInputPrompt;


    private void Awake()
    {
        cam = Camera.main;

    }

    private void Update()
    {

        if (isMoving == false)
        {
            Vector3 dir;
            if (Keyboard.current.dKey.wasPressedThisFrame)
            {
                dir = Vector3.right - Vector3.forward;
                dir.Normalize();
                CharacterSpriteRenderer.sprite = walkRightSprite;
                targetPosition = transform.position + dir * 1.85f;
                dInputPrompt.color = new Color(0x75/255f, 1, 0xEC/255f);
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
                aInputPrompt.color = new Color(0x75/255f, 1, 0xEC/255f);
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
                wInputPrompt.color = new Color(0x75/255f, 1, 0xEC/255f);
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
                sInputPrompt.color = new Color(0x75/255f, 1, 0xEC/255f);
                wInputPrompt.DOFade(0, 0.05f);
                aInputPrompt.DOFade(0, 0.05f);
                dInputPrompt.DOFade(0, 0.05f);
            }
            else
            {
                CharacterSpriteRenderer.sprite = walkIdleSprite;
            }
        }

        if (targetPosition == Vector3.zero || targetPosition == transform.position)
        {
            if (isMoving)
            {
                ResetInputPrompts();
            }

            isMoving = false;
        }
        else
        {
            isMoving = true;
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationSpeed * Time.deltaTime);
        }
    }

    private void ResetInputPrompts()
    {
        wInputPrompt.color = Color.white;
        aInputPrompt.color = Color.white;
        sInputPrompt.color = Color.white;
        dInputPrompt.color = Color.white;
    }
}
