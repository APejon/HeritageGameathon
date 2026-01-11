using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button instructionButton;
    [SerializeField] private RectTransform instructions;
    [SerializeField] private RectTransform arrowImage;
    private Boolean _hidden;

    void Start()
    {
        _hidden = true;
    }

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
        quitButton.onClick.AddListener(OnQuitButtonClicked);
        instructionButton.onClick.AddListener(OnInstructionsClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
    }

    private void OnQuitButtonClicked()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
    }

    private void OnPlayButtonClicked()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
        SceneManager.LoadScene("Level1");
    }

    private void OnInstructionsClicked()
    {
        if (_hidden)
        {
            instructions.DOAnchorPos(new Vector3(-600, -360, 0), 0.5f);
            arrowImage.DORotate(new Vector3(0, 0, 180), 0.5f);
            _hidden = false;
        }
        else
        {
            instructions.DOAnchorPos(new Vector3(0, -360, 0), 0.5f);
            arrowImage.DORotate(new Vector3(0, 0, 0), 0.5f);
            _hidden = true;
        }
    }
}
