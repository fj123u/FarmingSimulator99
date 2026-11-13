using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public float gameDuration = 120f;
    public int harvestGoal = 0;
    public string endSceneName = "EndScreen";
    public ScoreManager scoreManager;
    public TextMeshProUGUI timerText;
    private float remainingTime;
    private bool isGameOver = false;
    void Start()
    {
        Time.timeScale = 1f;
        remainingTime = gameDuration;
        UpdateTimerText();
    }
    void Update()
    {
        if (isGameOver)
        {
            return;
        }
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            UpdateTimerText();
            EndGame();
            return;
        }
        UpdateTimerText();
        if (harvestGoal > 0 && scoreManager.GetScore() >= harvestGoal)
        {
            EndGame();
        }
    }
    private void EndGame()
    {
        isGameOver = true;
        HighScoreStorage.SaveScore(scoreManager.GetScore());
        SceneManager.LoadScene(endSceneName);
    }
    private void UpdateTimerText()
    {
        if (timerText == null)
        {
            return;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60f);
        int seconds = Mathf.FloorToInt(remainingTime % 60f);
        timerText.text = minutes + ":" + seconds.ToString("00");
    }
}