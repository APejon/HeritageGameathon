using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MessagesConcept : MonoBehaviour
{
    private string[] _startingStrings;
    private int _index;
    private Coroutine _currentText;
    [SerializeField] CanvasGroup _resources;
    [SerializeField] CanvasGroup _compass;
    [SerializeField] TextMeshProUGUI _text;
    [SerializeField] CanvasGroup _fade;
    public static MessagesConcept instance;
    

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _resources.alpha = 0;
        _compass.alpha = 0;
        _startingStrings = new string[10];
        _startingStrings[0] = "Welcome to One Step At A Time.";
        _startingStrings[1] = "You are a travelling merchant";
        _startingStrings[2] = "due to your curiousity and hubris";
        _startingStrings[3] = "you've been separated from your clan";
        _startingStrings[4] = "find your way back to the village.";
        _startingStrings[5] = "Be mindful of your resources,";
        _startingStrings[6] = "you are only human afterall.";
        _startingStrings[7] = "Remember your training and survive,";
        _startingStrings[8] = "as what you have, will not be enough";
        _startingStrings[9] = "Good luck, make it home safe";
        _index = -1;
        _currentText = StartCoroutine(InitialMessages());
    }

    IEnumerator InitialMessages()
    {
        _index++;
        _text.text = _startingStrings[_index];
        AudioManager.instance.playSFX(AudioManager.soundEffect.GAMEEND, true);
        if (_index == 5)
        {
            _resources.DOFade(1f, 0.5f);
        }
        else if (_index == 7)
        {
            _resources.DOFade(0f, 0.5f);
            _compass.DOFade(1f, 0.5f);
        }
        else if (_index == 8)
        {
            _fade.DOFade(0f, 1f);
        }
        else if (_index == 9)
        {
            GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 202f), 0.5f);
            _compass.DOFade(0f, 0.5f);
            _resources.DOFade(1f, 0.5f);
        }
        yield return new WaitForSeconds(3f);
        if (_index >= _startingStrings.Length - 1)
            yield return null;
        else
            yield return InitialMessages();
    }

    public void SetText(string textToSet)
    {
        _text.text = textToSet;
    }

    void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (_index < _startingStrings.Length - 1)
            {
                StopCoroutine(_currentText);
                _currentText = StartCoroutine(InitialMessages());
            }
        }
    }
}
