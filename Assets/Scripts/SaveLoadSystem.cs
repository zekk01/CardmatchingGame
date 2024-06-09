using UnityEngine;

public static class SaveLoadSystem
{
    private const string ScoreKey = "Score";

    public static void SaveProgress(int score)
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.Save();
    }

    public static int LoadProgress()
    {
        return PlayerPrefs.GetInt(ScoreKey, 0);
    }
}
