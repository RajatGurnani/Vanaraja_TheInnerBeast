using UnityEngine;

public class GameOverInterstitial : MonoBehaviour
{
    public float interstitialTime = 5f;

    public void Awake()
    {
        if (!AdsManager.Instance.interstitialAd.CanShowAd())
        {
            AdsManager.Instance.RequestAndLoadInterstitial();
            enabled = false;
        }
        else
        {
            Invoke(nameof(ShowInterstitial), interstitialTime);
        }
    }

    public void ShowInterstitial()
    {
        AdsManager.Instance.ShowInterstitial();
    }
}