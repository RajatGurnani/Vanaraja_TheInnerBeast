using System.Collections;
using UnityEngine;

public class GameOverInterstitial : MonoBehaviour
{
    public float interstitialTime = 5f;

    Coroutine coroutine;
    public bool isRunning = true;

    public void Awake()
    {
        coroutine = StartCoroutine(nameof(ShowInterstitial));
    }

    IEnumerator ShowInterstitial()
    {
        yield return new WaitForSeconds(interstitialTime);
        isRunning = false;
        AdsManager.Instance.ShowInterstitial();
    }

    private void OnDisable()
    {
        if (isRunning)
        {
            StopCoroutine(coroutine);
        }
    }
}