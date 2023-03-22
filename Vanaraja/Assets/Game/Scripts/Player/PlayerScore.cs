using UnityEngine;

public class PlayerScore : MonoBehaviour
{
    public int shards = 0;
    public float score = 0;
    public float highscore = 0;
    public float multiplier = 0f;
    public float multiplierCap = 3f;
    public int nearMissScore;

    public PlayerMovement playerMovement;
    public const string SHARDS_COUNT = "SHARDS_COUNT";
    public const string HIGHSCORE = "HIGHSCORE";

    public void GameStarted()
    {

    }

    public void GameOver()
    {
        SaveStats();
        LeaderboardSystem.AddScoreToLeaderboard(GPGSIds.leaderboard_lifetime_high_score,(int)score);
    }

    public void UpdateShard()
    {
        shards++;
    }

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(SHARDS_COUNT))
        {
            PlayerPrefs.SetInt(SHARDS_COUNT, 0);
            PlayerPrefs.Save();
        }
        if (!PlayerPrefs.HasKey(HIGHSCORE))
        {
            PlayerPrefs.SetInt(HIGHSCORE, 0);
            PlayerPrefs.Save();
        }
        highscore= PlayerPrefs.GetFloat(HIGHSCORE,0);
        shards = PlayerPrefs.GetInt(SHARDS_COUNT, 0);
    }

    public void SaveStats()
    {
        if (score>highscore)
        {
            highscore = score;
            PlayerPrefs.SetFloat(HIGHSCORE, highscore);
        }
        PlayerPrefs.SetInt(SHARDS_COUNT, shards);
        PlayerPrefs.Save();
    }

    public void UpdateScore(int value)
    {
        score += ((1 + multiplier) * value);
    }

    public void UpdateMultiplier(bool _active)
    {
        Debug.Log($"Distance- {playerMovement.distance}, score- {score}");
        multiplier = _active ? Mathf.Clamp(multiplier + 0.5f, 0f, multiplierCap) : 0f;
    }

    private void OnEnable()
    {
        PlayerCollision.ShardCollected += UpdateShard;
        GameManager.GameOver += GameOver;
        GameManager.GameStarted += GameStarted;
        PlayerMovement.PlayerMoved += UpdateScore;
        PlayerNearMiss.NearMiss += UpdateMultiplier;
    }

    private void OnDisable()
    {
        PlayerCollision.ShardCollected -= UpdateShard;
        GameManager.GameOver -= GameOver;
        GameManager.GameStarted -= GameStarted;
        PlayerMovement.PlayerMoved -= UpdateScore;
        PlayerNearMiss.NearMiss -= UpdateMultiplier;
    }

    private void OnDestroy()
    {
        SaveStats();
    }
}
