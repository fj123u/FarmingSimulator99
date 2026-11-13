using TMPro;
using UnityEngine;
public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public string scoreLabel = "Score : ";
    private int currentScore = 0;
    void Start()
    {
        UpdateScoreText();
    }
    public void AddScore(int amount)
    {
        currentScore += amount;
        UpdateScoreText();
    }
    public int GetScore()
    {
        return currentScore;
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = scoreLabel + currentScore;
        }
    }
}