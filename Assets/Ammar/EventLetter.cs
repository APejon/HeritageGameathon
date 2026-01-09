using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using DG.Tweening;

public class EventLetter : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] CanvasGroup _letterGroup;

    void Start()
    {
        
    }

    void Update()
    {
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            ShowLetter();
        }
        if (Keyboard.current.hKey.wasPressedThisFrame)
        {
            HideLetter();
        }

    }

    private void ShowLetter()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(_letterGroup.DOFade(1f, 1f));
        seq.Join(_image.rectTransform.DOAnchorPos(new Vector2(0f, 0f), 1f).SetEase(Ease.OutQuad));
    }

    private void HideLetter()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(_letterGroup.DOFade(0f, 1f));
        seq.Join(_image.rectTransform.DOAnchorPos(new Vector2(0f, -460f), 1f).SetEase(Ease.OutQuad));
    }
}
