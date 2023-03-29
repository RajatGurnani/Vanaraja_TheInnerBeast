using System;
using UnityEngine;
using Yodo1.MAS;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance { set; get; }

    public Action<bool> RewardFuntion;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log(SystemInfo.deviceUniqueIdentifier);
        Yodo1U3dMasCallback.OnSdkInitializedEvent += (success, error) =>
        {
            Debug.Log("[Yodo1 Mas] OnSdkInitializedEvent, success:" + success + ", error: " + error.ToString());
            if (success)
            {
                Debug.Log("[Yodo1 Mas] The initialization has succeeded");

                FindObjectOfType<StartScene>().StartSplash();
                InitializeInterstitial();
                InitializeRewardedAds();

            }
            else
            {
                FindObjectOfType<StartScene>().StartSplash();
                Debug.Log("[Yodo1 Mas] The initialization has failed");
            }
        };

        Yodo1AdBuildConfig config = new Yodo1AdBuildConfig().enableUserPrivacyDialog(true);
        Yodo1U3dMas.SetAdBuildConfig(config);

        Yodo1U3dMas.InitializeSdk();
    }

    /**************** Interstitials code ****************/

    public void InitializeInterstitial()
    {
        // Instantiate
        Yodo1U3dInterstitialAd.GetInstance();

        // Ad Events
        Yodo1U3dInterstitialAd.GetInstance().OnAdLoadedEvent += OnInterstitialAdLoadedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdLoadFailedEvent += OnInterstitialAdLoadFailedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdOpenedEvent += OnInterstitialAdOpenedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdOpenFailedEvent += OnInterstitialAdOpenFailedEvent;
        Yodo1U3dInterstitialAd.GetInstance().OnAdClosedEvent += OnInterstitialAdClosedEvent;

        // Load an ad
        LoadInterstitial();
    }

    public void LoadInterstitial()
    {
        Yodo1U3dInterstitialAd.GetInstance().LoadAd();
    }

    public void ShowInterstitial()
    {

        if (Yodo1U3dInterstitialAd.GetInstance().IsLoaded())
        {
            Yodo1U3dInterstitialAd.GetInstance().ShowAd("Your Placement");
        }
        else
        {
            Debug.Log("[Yodo1 Mas] Interstitial ad has not been cached.");
        }
    }

    private void OnInterstitialAdLoadedEvent(Yodo1U3dInterstitialAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdLoadedEvent event received" + ad.GetHashCode());
    }

    private void OnInterstitialAdLoadFailedEvent(Yodo1U3dInterstitialAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdLoadFailedEvent event received with error: " + adError.ToString());
    }

    private void OnInterstitialAdOpenedEvent(Yodo1U3dInterstitialAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdOpenedEvent event received");
    }

    private void OnInterstitialAdOpenFailedEvent(Yodo1U3dInterstitialAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdOpenFailedEvent event received with error: " + adError.ToString());
        Invoke(nameof(LoadDelayInterstitial), 1f);
    }

    private void OnInterstitialAdClosedEvent(Yodo1U3dInterstitialAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnInterstitialAdClosedEvent event received");
        Invoke(nameof(LoadDelayInterstitial), 1f);
    }

    public void LoadDelayInterstitial()
    {
        LoadInterstitial();
    }

    /**************** Rewarded ads code ****************/

    public void InitializeRewardedAds()
    {
        Yodo1U3dRewardAd.GetInstance();

        // Ad Events
        Yodo1U3dRewardAd.GetInstance().OnAdLoadedEvent += OnRewardAdLoadedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdLoadFailedEvent += OnRewardAdLoadFailedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdOpenedEvent += OnRewardAdOpenedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdOpenFailedEvent += OnRewardAdOpenFailedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdClosedEvent += OnRewardAdClosedEvent;
        Yodo1U3dRewardAd.GetInstance().OnAdEarnedEvent += OnRewardAdEarnedEvent;

        // Load an ad
        LoadRewardedAd();
    }

    public void LoadRewardedAd()
    {
        Yodo1U3dRewardAd.GetInstance().LoadAd();
    }

    public bool CanShowRewardedAd()
    {
        return Yodo1U3dRewardAd.GetInstance().IsLoaded();
    }

    public void ShowRewardedVideo(Action<bool> _function)
    {
        RewardFuntion = _function;
        if (Yodo1U3dRewardAd.GetInstance().IsLoaded())
        {
            Yodo1U3dRewardAd.GetInstance().ShowAd();
        }
        else
        {
            Debug.Log("[Yodo1 Mas] Reward video ad has not been cached.");
        }
    }

    private void OnRewardAdLoadedEvent(Yodo1U3dRewardAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnRewardAdLoadedEvent event received" + ad.GetHashCode());
    }

    private void OnRewardAdLoadFailedEvent(Yodo1U3dRewardAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnRewardAdLoadFailedEvent event received with error: " + adError.ToString());
    }

    private void OnRewardAdOpenedEvent(Yodo1U3dRewardAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnRewardAdOpenedEvent event received");
    }

    private void OnRewardAdOpenFailedEvent(Yodo1U3dRewardAd ad, Yodo1U3dAdError adError)
    {
        Debug.Log("[Yodo1 Mas] OnRewardAdOpenFailedEvent event received with error: " + adError.ToString());
        Invoke(nameof(LoadDelayRewarded), 1f);
    }

    private void OnRewardAdClosedEvent(Yodo1U3dRewardAd ad)
    {
        Debug.Log("[Yodo1 Mas] OnRewardAdClosedEvent event received");
        Invoke(nameof(LoadDelayRewarded), 1f);
    }
    public void LoadDelayRewarded()
    {
        LoadRewardedAd();
    }

    private void OnRewardAdEarnedEvent(Yodo1U3dRewardAd ad)
    {
        RewardFuntion?.Invoke(true);
        RewardFuntion = null;
        Debug.Log("[Yodo1 Mas] OnRewardAdEarnedEvent event received");
    }
}