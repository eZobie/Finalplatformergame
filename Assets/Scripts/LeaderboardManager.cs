using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LeaderboardManager : MonoBehaviour
{
    public Text leaderboardText;
    private List<float> leaderboardTimes = new List<float>();

    void Start()
    {
        // Load saved leaderboard data (if any)
        LoadLeaderboard();
    }

    public void AddTime(float time)
    {
        leaderboardTimes.Add(time);
        leaderboardTimes.Sort(); // Sort the times
        SaveLeaderboard();
        UpdateLeaderboardText();
    }

    void UpdateLeaderboardText()
    {
        leaderboardText.text = "Leaderboard:\n";
        for (int i = 0; i < leaderboardTimes.Count; i++)
        {
            leaderboardText.text += (i + 1) + ". " + FormatTime(leaderboardTimes[i]) + "\n";
        }
    }

    string FormatTime(float time)
    {
        int minutes = (int)(time / 60);
        int seconds = (int)(time % 60);
        float milliseconds = (time * 1000) % 1000;
        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    void SaveLeaderboard()
    {
        // Save leaderboard data (using PlayerPrefs, JSON, or any other method)
        // For simplicity, let's use PlayerPrefs in this example
        for (int i = 0; i < leaderboardTimes.Count; i++)
        {
            PlayerPrefs.SetFloat("LeaderboardTime" + i, leaderboardTimes[i]);
        }
        PlayerPrefs.Save();
    }

    void LoadLeaderboard()
    {
        // Load leaderboard data (using PlayerPrefs, JSON, or any other method)
        // For simplicity, let's use PlayerPrefs in this example
        leaderboardTimes.Clear();
        for (int i = 0; i < 10; i++) // Load top 10 leaderboard entries
        {
            if (PlayerPrefs.HasKey("LeaderboardTime" + i))
            {
                float time = PlayerPrefs.GetFloat("LeaderboardTime" + i);
                leaderboardTimes.Add(time);
            }
        }
    }
}
