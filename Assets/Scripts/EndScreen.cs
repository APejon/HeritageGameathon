using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [SerializeField] private Button quitButton1;
    [SerializeField] private Button quitButton2;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private CanvasGroup gameOverPopup;
    [SerializeField] private CanvasGroup gameEndPopup;
    [SerializeField] private ResourceBars resourceBars;
    [SerializeField] private GameObject fadeBackground;


    private void Awake()
    {
        resourceBars.death += OnDeath;
        VictorySequenceEnabler.OnVictorySequenceComplete += OnVictorySequenceComplete;
    }

    private void OnDestroy()
    {
        quitButton1.onClick.RemoveListener(Application.Quit);
        quitButton2.onClick.RemoveListener(Application.Quit);
        mainMenuButton.onClick.RemoveListener(LoadMainMenu);
        restartButton.onClick.RemoveListener(RestartGame);
        VictorySequenceEnabler.OnVictorySequenceComplete -= OnVictorySequenceComplete;
        resourceBars.death -= OnDeath;
    }

    private void OnVictorySequenceComplete()
    {
        fadeBackground.SetActive(true);
        gameObject.SetActive(true);
        gameEndPopup.gameObject.SetActive(true);
        gameEndPopup.DOFade(0, 0.5f).From();
        gameEndPopup.GetComponent<RectTransform>().DOAnchorPosY(-200, 0.5f).From();
        VictorySequenceEnabler.OnVictorySequenceComplete -= OnVictorySequenceComplete;


        quitButton2.onClick.AddListener(Application.Quit);
        mainMenuButton.onClick.AddListener(LoadMainMenu);
    }

    private void LoadMainMenu()
    {
        VictorySequenceEnabler.OnVictorySequenceComplete -= OnVictorySequenceComplete;

        SceneManager.LoadScene("MainMenu");
    }

    private void OnDeath()
    {
        fadeBackground.SetActive(true);
        resourceBars.death -= OnDeath;
        Debug.Log("Showing end screen");
        gameObject.SetActive(true);
        gameOverPopup.gameObject.SetActive(true);
        gameOverPopup.DOFade(0, 0.5f).From();
        gameOverPopup.GetComponent<RectTransform>().DOAnchorPosY(-200, 0.5f).From();


        quitButton1.onClick.AddListener(Application.Quit);
        restartButton.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        AudioManager.instance.stopAllSources();
        transform.DOScale(0, 0.5f).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
