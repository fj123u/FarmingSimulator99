using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EndScreen : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public GameObject newRecordLabel;
    public string gameSceneName = "Game";
    public string menuSceneName = "MainMenu";
    void Start()
    {
        Time.timeScale = 1f;
        if (scoreText != null)
        {
            scoreText.text = "Score : " + HighScoreStorage.GetLastScore();
        }
        if (bestScoreText != null)
        {
            bestScoreText.text = "Meilleur score : " + HighScoreStorage.GetBestScore();
        }
        if (newRecordLabel != null)
        {
            newRecordLabel.SetActive(HighScoreStorage.IsLastScoreRecord());
        }
    }
    public void ReplayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(menuSceneName);
    }
}