using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string gameSceneName = "Game";
    public TextMeshProUGUI bestScoreText;
    public GameObject quitButton;
    void Start()
    {
        Time.timeScale = 1f;
        if (bestScoreText != null)
        {
            bestScoreText.text = "Meilleur score : " + HighScoreStorage.GetBestScore();
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}