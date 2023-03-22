using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class LeaderboardSystem : MonoBehaviour
{
    public static LeaderboardSystem instance;
    public static bool connectedToGooglePlay;

    void Awake()
    {
        instance = this;
        PlayGamesPlatform.Activate();
    }

    void Start()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    public void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log("leaderboard Test");
        if (status == SignInStatus.Success)
        {
            connectedToGooglePlay = true;
        }
        else
        {
            connectedToGooglePlay = false;
        }
    }

    #region Leaderboard
    public void ShowLeaderboardUI()
    {
        if (!connectedToGooglePlay)
        {
            PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
        }
        Social.ShowLeaderboardUI();
    }

    public static void AddScoreToLeaderboard(string leaderboardID, long score)
    {
        if (connectedToGooglePlay)
        {
            Debug.Log("Leaderboard Score Sent");
            Social.ReportScore(score, leaderboardID, success => { });
        }
    }
    #endregion
}