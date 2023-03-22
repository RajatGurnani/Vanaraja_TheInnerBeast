using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    RetryCounter retryCounter;
    GameManager gameManger;
    PlayerScore playerScore;
    PlayerMovement playerMovement;
    PlayerNearMiss playerNearMiss;

    public GameObject menuBar;

    [Header("Game Start")]
    public GameObject mainMenuPanel;
    public GameObject commonPanel;

    [Header("Gameplay")]
    public GameObject gameplayPanel;
    public TMP_Text distanceText;
    public TMP_Text scoreText;
    public float pulsatingTime = 0.3f;
    public TMP_Text multiplierText;
    public Image multiplierSlider;
    public GameObject multiplier;
    public TMP_Text shardText;
    public Image shardImage;

    [Header("Gameover")]
    public GameObject gameOverPanel;
    public TMP_Text gameoverScoreText;
    public TMP_Text gameoverHighScoreText;

    [Header("PauseMenu")]
    public GameObject tapToContinuePanel;
    public GameObject pauseMenuPanel;


    [Header("Continue Panel with Timer")]
    public int continuePanelTimer = 0;
    public int continuePanelCooldown = 5;
    public TMP_Text continueTimerText;
    public TMP_Text continueShardsText;
    public GameObject continueWithTimePanel;

    static bool restartGame = false;

    Coroutine coroutine;

    private void Start()
    {
        retryCounter = FindObjectOfType<RetryCounter>();
        multiplierText.rectTransform.transform.DOPunchScale(0.3f * Vector3.one, pulsatingTime, 0, 1).SetLoops(-1);//.OnKill(() => multiplier.transform.localScale = Vector3.one);
        gameManger = GameManager.Instance;
        playerMovement = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerMovement>();
        playerScore = playerMovement.GetComponent<PlayerScore>();
        playerNearMiss = playerMovement.GetComponent<PlayerNearMiss>();

        if (restartGame)
        {
            restartGame = false;
            StartGame();
        }
    }

    private void Update()
    {
        UpdateShard();
        UpdateDistance();
        UpdateScore();
        UpdateMultiplier();
    }

    public void UpdateShard()
    {
        shardText.text = playerScore.shards.ToString();
        continueShardsText.text = playerScore.shards.ToString();
        if (playerScore.shards != 0)
        {
            if (playerScore.shards % 2 == 0)
            {
                shardImage.fillAmount = 1f;
            }
            else
            {
                shardImage.fillAmount = 0.5f;
            }
        }
    }

    public void UpdateScore()
    {
        multiplierSlider.fillAmount = playerNearMiss.nearMissTimer / playerNearMiss.nearMissCountdown;
        scoreText.text = $"{playerScore.score:0}";
    }

    public void UpdateDistance()
    {
        distanceText.text = $"Distance- {playerMovement.distance:0}m";
    }

    public void StartGame()
    {
        commonPanel.SetActive(false);
        mainMenuPanel.SetActive(false);
        continueWithTimePanel.SetActive(false);
        gameOverPanel.SetActive(false);
        gameplayPanel.SetActive(true);
        menuBar.SetActive(false);
        GameManager.GameStarted?.Invoke();
    }

    public void MainMenu()
    {
        gameManger.ChangeScene();
    }

    public void ContinueGameUsingShards()
    {
        if (playerScore.shards > 4)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
            playerScore.shards -= 5;
            playerScore.SaveStats();
            StartGame();
        }
    }

    public void ReplayGame()
    {
        restartGame = true;
        MainMenu();
    }

    public void PauseGame() => GameManager.GamePaused?.Invoke(true);

    public void ExitGame() => Application.Quit();

    public void GameOver()
    {
        Debug.Log("Game Over");
        if (retryCounter.CanRetry())
        {
            coroutine = StartCoroutine(nameof(GameOverTimer));
        }
        else
        {
            GameOverUI();
            continueWithTimePanel.SetActive(false);
            commonPanel.SetActive(true);
            gameOverPanel.SetActive(true);
        }
    }

    public void GameOverUI()
    {
        playerScore.SaveStats();
        gameoverScoreText.text = $"{playerScore.score:0}";
        gameoverHighScoreText.text = $"BestScore- {playerScore.highscore:0}";
    }

    /// <summary>
    /// Open Google play leaderboard
    /// </summary>
    public void OpenLeaderboard()
    {
        LeaderboardSystem.instance.ShowLeaderboardUI();
    }

    IEnumerator GameOverTimer()
    {
        continuePanelTimer = continuePanelCooldown;
        continueWithTimePanel.SetActive(true);
        while (continuePanelTimer > 0)
        {
            continueTimerText.text = continuePanelTimer.ToString();
            Debug.Log($"called + {continuePanelTimer}");
            --continuePanelTimer;
            yield return new WaitForSeconds(1f);
        }
    }

    public void WatchAdToContinue()
    {
        Debug.Log("ad possible- " + AdsManager.Instance.rewardedAd.CanShowAd());
        if (AdsManager.Instance.rewardedAd.CanShowAd())
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
            }
        }
        AdsManager.Instance.ShowRewardedVideo(ContinueForAd);
    }

    public void ContinueForAd(bool value)
    {
        if (true)
        {
            retryCounter.IncrementRetries();
            playerScore.shards += 5;
            if (playerScore.shards > 4)
            {
                playerScore.shards -= 5;
                playerScore.SaveStats();
                StartGame();
            }
        }
    }

    public void NearMissUI(bool _active)
    {
        if (_active)
        {
            multiplier.SetActive(true);
            multiplierText.text = $"x{playerScore.multiplier:F2}";
        }
        else
        {
            multiplier.SetActive(false);
        }
    }

    public void UpdateMultiplier()
    {
        multiplierText.text = $"x{playerScore.multiplier}";
    }

    public void GamePaused(bool value)
    {
        if (value)
        {
            gameplayPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
            tapToContinuePanel.SetActive(true);
        }
        else
        {
            gameplayPanel.SetActive(true);
            tapToContinuePanel.SetActive(false);
        }
    }

    private void OnEnable()
    {
        PlayerNearMiss.NearMiss += NearMissUI;
        GameManager.GameOver += GameOver;
        GameManager.GamePaused += GamePaused;
    }

    private void OnDisable()
    {
        PlayerNearMiss.NearMiss -= NearMissUI;
        GameManager.GameOver -= GameOver;
        GameManager.GamePaused -= GamePaused;
    }

    public void UnpauseGame()
    {
        GameManager.GamePaused?.Invoke(false);
    }

    public void Continue()
    {
        GameManager.GameStarted?.Invoke();
    }
}
