using System;
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


    private void Awake()
    {
        cam = Camera.main;

    }

    private void Update()
    {
        Vector3 dir;
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            dir = Vector3.right - Vector3.forward;
            dir.Normalize();
            CharacterSpriteRenderer.sprite = walkRightSprite;
            targetPosition = transform.position + dir * 1.75f;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            dir = -Vector3.right + Vector3.forward;
            dir.Normalize();
            CharacterSpriteRenderer.sprite = walkLeftSprite;
            targetPosition = transform.position + dir * 1.75f;
        }
        else if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            dir = Vector3.right + Vector3.forward;
            dir.Normalize();
            CharacterSpriteRenderer.sprite = walkIdleSprite;
            targetPosition = transform.position + dir * 1.75f;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            dir = -Vector3.right - Vector3.forward;
            dir.Normalize();
            CharacterSpriteRenderer.sprite = walkIdleSprite;
            targetPosition = transform.position + dir * 1.75f;
        }
        else
        {
            CharacterSpriteRenderer.sprite = walkIdleSprite;
        }
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, animationSpeed * Time.deltaTime);
    }
}
