using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Sprite walkLeftSprite;
    [SerializeField] private Sprite walkRightSprite;
    [SerializeField] private Sprite walkIdleSprite;
    public SpriteRenderer CharacterSpriteRenderer;
    private Camera cam;


    private void Awake()
    {
        cam = Camera.main;

    }

    private void Update()
    {
        var dir = Vector3.zero;
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            dir = Vector3.right - Vector3.forward;
            CharacterSpriteRenderer.sprite = walkRightSprite;
        }
        else if (Keyboard.current.aKey.wasPressedThisFrame)
        {
            dir = -Vector3.right + Vector3.forward;
            CharacterSpriteRenderer.sprite = walkLeftSprite;
        }
        else if (Keyboard.current.wKey.wasPressedThisFrame)
        {
            dir = Vector3.right + Vector3.forward;
            CharacterSpriteRenderer.sprite = walkIdleSprite;
        }
        else if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            dir = -Vector3.right - Vector3.forward;
            CharacterSpriteRenderer.sprite = walkIdleSprite;
        }
        else
        {
            CharacterSpriteRenderer.sprite = walkIdleSprite;
        }
        dir.Normalize();
        dir.y = 0;
        transform.position += dir * 1.75f;
    }
}
