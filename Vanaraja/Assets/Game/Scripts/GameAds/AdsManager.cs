using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public GameObject gdprPanel;

    private const string userConsent = "UserConsent";
    private const string ccpaConsent = "CcpaConsent";

    public string ccpaValue = "0";
    public string userValue = "0";

    public InterstitialAd interstitialAd;
    public RewardedAd rewardedAd;

    public string privacyUrl = "https://sites.google.com/view/nerdyquest/home";
    private string rewardedID= "ca-app-pub-7409912642531316/9949235493";
    private string interstitialID = "ca-app-pub-7409912642531316/4845082627";

    public Action<bool> RewardFunction;

    public static AdsManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey(userConsent) && PlayerPrefs.HasKey(ccpaConsent))
        {
            userValue = PlayerPrefs.GetString(userConsent, "0");
            ccpaValue = PlayerPrefs.GetString(ccpaConsent, "0");
            gdprPanel.SetActive(false);
            Init();
        }
        else
        {
            gdprPanel.SetActive(true);
        }
    }

    public void Init()
    {
        FindObjectOfType<StartScene>().StartSplash();
        RequestConfiguration.Builder requestConfiguration = new RequestConfiguration.Builder();
        requestConfiguration.SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified);
        requestConfiguration.SetMaxAdContentRating(MaxAdContentRating.Unspecified);
        requestConfiguration.SetTagForUnderAgeOfConsent(TagForUnderAgeOfConsent.Unspecified);
        MobileAds.SetRequestConfiguration(requestConfiguration.build());
        MobileAds.Initialize(initStatus => { });
        RequestAndLoadRewarded();
    }

    public void GDPRInput(bool _value)
    {
        // if true then allow personalized ads
        StartCoroutine(DelayedInput(_value));
    }

    public IEnumerator DelayedInput(bool _value)
    {
        yield return new WaitForSeconds(1f);
        if (_value)
        {
            userValue = "0";
            ccpaValue = "0";
            PlayerPrefs.SetString(userConsent, "0");
            PlayerPrefs.SetString(ccpaConsent, "0");
        }
        else
        {
            userValue = "1";
            ccpaValue = "1";
            PlayerPrefs.SetString(userConsent, "1");
            PlayerPrefs.SetString(ccpaConsent, "1");
        }
        Init();
        PlayerPrefs.Save();
        gdprPanel.SetActive(false);
    }

    public AdRequest GetAdRequestBuild()
    {
        return new AdRequest.Builder().AddExtra("npa", userValue).AddExtra("rdp", ccpaConsent).AddExtra("is_designed_for_families", "false").Build();
    }

    #region Interstitial
    public void RequestAndLoadInterstitial()
    {
        InterstitialAd.Load(interstitialID, GetAdRequestBuild(), (InterstitialAd ad, LoadAdError loadAdError) =>
        {
            if (loadAdError != null)
            {
                return;
            }

            interstitialAd = ad;

            ad.OnAdFullScreenContentClosed += () =>
            {
                ad.Destroy();
            };

            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                Debug.Log("Rewarded intersitial ad failed to show with error: " + error.GetMessage());
                ad.Destroy();
            };
        });
    }

    public void ShowInterstitial()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            RequestAndLoadInterstitial();
        }
    }

    #endregion

    #region Rewarded
    public void RequestAndLoadRewarded()
    {
        RewardedAd.Load(rewardedID, GetAdRequestBuild(), (RewardedAd ad, LoadAdError loadError) =>
        {
            if (loadError != null)
            {
                Debug.Log("Rewarded ad failed to load with error: " + loadError.GetMessage());
                return;
            }
            else if (ad == null)
            {
                Debug.Log("Rewarded ad failed to load.");
                return;
            }

            rewardedAd = ad;
        });
    }

    public void ShowRewardedVideo(Action<bool> Function)
    {
        RewardFunction = Function;
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show((Reward reward) =>
            {
                Invoke(nameof(TrueFunction), 1f);
            });
        }
        else
        {
            RequestAndLoadRewarded();
        }
    }

    public void TrueFunction()
    {
        RewardFunction?.Invoke(true);
    }

    public void UserEarnedReward(object sender, Reward reward)
    {
        RewardFunction(true);
    }

    public void OnAdClosed(object sender, EventArgs eventArgs)
    {
        RequestAndLoadRewarded();
    }
    #endregion

    public void OpenPrivacyLink() => Application.OpenURL(privacyUrl);

    private void OnEnable()
    {
        //GameManager.GameOver += ShowInterstitial;
        GameManager.GameStarted += RequestAndLoadInterstitial;
        GameManager.GameStarted += RequestAndLoadRewarded;
    }

    private void OnDisable()
    {
        //GameManager.GameOver -= ShowInterstitial;
        GameManager.GameStarted -= RequestAndLoadInterstitial;
        GameManager.GameStarted -= RequestAndLoadRewarded;
    }
}
