using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public bool invincible = false;
    public GameManager gameManager;
    public bool gameOver = false;

    public static System.Action ShardCollected;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Tree) || other.CompareTag(Tags.Stone)) && !invincible && !gameOver)
        {
            Debug.Log("Double Event");
            GameManager.GameOver?.Invoke();
        }

        if (other.CompareTag(Tags.LunarShard))
        {
            ShardCollected?.Invoke();
        }
    }

    public void GameStarted() => gameOver = false;
    public void GameOver() => gameOver = true;

    public void IsInvincible(bool value)
    {
        Debug.Log(value ? "invincible" : "not invincible");
        invincible = value;
    }

    private void OnEnable()
    {
        GameManager.GameOver += GameOver;
        GameManager.GameStarted += GameStarted;
        InjectionBar.GoldenPeriod += IsInvincible;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= GameOver;
        GameManager.GameStarted -= GameStarted;
        InjectionBar.GoldenPeriod -= IsInvincible;
    }
}
