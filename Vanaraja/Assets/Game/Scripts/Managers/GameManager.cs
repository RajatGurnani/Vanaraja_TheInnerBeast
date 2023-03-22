using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static System.Action GameOver;
    public static System.Action GameStarted;
    public static System.Action<bool> GamePaused;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
    }

    private void OnApplicationFocus2(bool focus)
    {
        if (!focus)
        {
            GamePaused?.Invoke(focus);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void GameOver_()
    {
        //Save player Stats and clear them afterwards
    }

    private void GameStarted_()
    {
        // Clear local Stats
    }

    private void OnEnable()
    {
        GameOver += GameOver_;
        GameStarted += GameStarted_;
    }

    private void OnDisable()
    {
        GameOver -= GameOver_;
        GameStarted -= GameStarted_;
    }
}
