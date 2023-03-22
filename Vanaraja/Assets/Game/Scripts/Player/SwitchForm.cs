using UnityEngine;

public class SwitchForm : MonoBehaviour
{
    /// <summary>
    /// Switch between wolf and human form
    /// </summary>
    public static System.Action<bool> SwitchStates;
    public static System.Action AboutToTurnIntoWolf;
    public bool isWolf;

    public float timer = 0f;
    public float humanTime = 20f;
    public float preWolfingTime = 2f;
    public float wolfTime = 10f;
    public float combinedTime = 0f;

    public bool aboutToTurnIntoWolf;
    public bool isPaused;

    public float normalizedValue;
    public float normalizedTransformationValue;
    public float normalizedWolfTime;

    private void Awake()
    {
        isPaused = true;
        NormalizedTime();
    }

    private void Update()
    {
        if (isPaused || Time.timeScale == 0)
            return;

        timer = Mathf.Repeat(timer + Time.deltaTime, combinedTime);
        if (!aboutToTurnIntoWolf)
        {
            if (humanTime - timer <= preWolfingTime)
            {
                aboutToTurnIntoWolf = true;
                AboutToTurnIntoWolf?.Invoke();
            }
        }

        if (!isWolf && timer >= humanTime)
        {
            isWolf = true;
            SwitchStates?.Invoke(true);
        }
        else if (isWolf && timer < humanTime)
        {
            isWolf = false;
            SwitchStates?.Invoke(false);
        }
        NormalizedTime();
    }

    public void NormalizedTime()
    {
        normalizedValue = timer / combinedTime;
        normalizedTransformationValue = (humanTime - preWolfingTime) / combinedTime;
        normalizedWolfTime = humanTime / combinedTime;
    }

    public void GameOver() => isPaused = true;
    public void GamePaused(bool value) => isPaused = value;
    public void GameStarted() => isPaused = false;
    private void OnValidate()
    {
        combinedTime = humanTime + wolfTime;
    }

    private void OnEnable()
    {
        GameManager.GameOver += GameOver;
        GameManager.GamePaused += GamePaused;
        GameManager.GameStarted += GameStarted;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= GameOver;
        GameManager.GamePaused -= GamePaused;
        GameManager.GameStarted -= GameStarted;
    }
}
