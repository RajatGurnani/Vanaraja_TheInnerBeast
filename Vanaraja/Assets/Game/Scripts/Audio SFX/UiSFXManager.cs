using System.Collections.Generic;
using UnityEngine;

public class UiSFXManager : MonoBehaviour
{
    public static UiSFXManager Instance;
    public AudioSource sfxSource;

    public AudioClip buttonClick;
    public AudioClip sliderTick;

    public enum SFXType
    {
        ButtonClick,
        SliderTick
    };

    public Dictionary<SFXType, AudioClip> dict = new Dictionary<SFXType, AudioClip>();

    public void Awake()
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

    public void PlaySFX(SFXType sfxType)
    {
        switch (sfxType)
        {
            case SFXType.ButtonClick:
                sfxSource.PlayOneShot(buttonClick);
                break;
            case SFXType.SliderTick:
                sfxSource.PlayOneShot(sliderTick);
                break;
            default:
                break;
        }
    }
}
