using UnityEngine;
public static class HighScoreStorage
{
    private const string BestScoreKey = "BestScore";
    private const string LastScoreKey = "LastScore";
    private const string LastScoreIsRecordKey = "LastScoreIsRecord";
    public static void SaveScore(int score)
    {
        bool isNewRecord = score > GetBestScore();
        if (isNewRecord)
        {
            PlayerPrefs.SetInt(BestScoreKey, score);
        }
        PlayerPrefs.SetInt(LastScoreKey, score);
        PlayerPrefs.SetInt(LastScoreIsRecordKey, isNewRecord ? 1 : 0);
        PlayerPrefs.Save();
    }
    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScoreKey, 0);
    }
    public static int GetLastScore()
    {
        return PlayerPrefs.GetInt(LastScoreKey, 0);
    }
    public static bool IsLastScoreRecord()
    {
        return PlayerPrefs.GetInt(LastScoreIsRecordKey, 0) == 1;
    }
}