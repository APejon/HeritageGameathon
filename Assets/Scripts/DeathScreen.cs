using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private RectTransform gameOverPopup;


    private void OnEnable()
    {
        GetComponentInChildren<CanvasGroup>().DOFade(0, 0.5f).From();
        gameOverPopup.DOAnchorPosY(-200, 0.5f).From();


        quitButton.onClick.AddListener(Application.Quit);
        restartButton.onClick.AddListener(RestartGame);
    }

    private void OnDisable()
    {
        quitButton.onClick.RemoveListener(Application.Quit);
        restartButton.onClick.RemoveListener(RestartGame);
    }

    public void RestartGame()
    {
        transform.DOScale(0, 0.5f).OnComplete(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
    }
}
