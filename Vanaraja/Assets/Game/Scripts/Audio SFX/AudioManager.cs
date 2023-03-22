using System;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Mixers")]
    public AudioMixer sfxMixer;
    public AudioMixer musicMixer;

    [Header("Audio Player Prefs")]
    public const string SFX_VOLUME = "SFX_VOLUME";
    public const string MUSIC_VOLUME = "MUSIC_VOLUME";
    public const string MASTER_VOLUME = "MASTER_VOLUME";


    [Header("Exposed Parameters")]
    public readonly string paramVolume = "Volume";

    public float master;
    public float sfx;
    public float music;

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
        FirstTimeLoad();
    }

    private void Start()
    {
        SetMasterVolume(master);
        SetMusicVolume(music);
        SetSFXVolume(sfx);
    }

    public void SetSFXVolume(float volume = 0.5f)
    {
        sfx = volume;
        sfxMixer.SetFloat(paramVolume, VolumeLinearToLog(volume));
        PlayerPrefs.SetFloat(SFX_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float volume = 0.5f)
    {
        music = volume;
        musicMixer.SetFloat(paramVolume, VolumeLinearToLog(volume));
        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float volume = 1f)
    {
        master = volume;
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat(MASTER_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public void FirstTimeLoad()
    {
        if (!PlayerPrefs.HasKey(SFX_VOLUME))
        {
            PlayerPrefs.SetFloat(SFX_VOLUME, 0.5f);
        }
        if (!PlayerPrefs.HasKey(MUSIC_VOLUME))
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, 0.5f);
        }
        if (!PlayerPrefs.HasKey(MASTER_VOLUME))
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME, 1f);
        }

        sfx = PlayerPrefs.GetFloat(SFX_VOLUME, 0.5f);
        music = PlayerPrefs.GetFloat(MUSIC_VOLUME, 0.5f);
        master = PlayerPrefs.GetFloat(MASTER_VOLUME, 1f);
    }

    /// <summary>
    /// Takes the linear value from 0-1 and returns the
    /// corresponding logarithmic value for audio
    /// </summary>
    /// <param name="linear"></param>
    /// <returns>corresponmding logarithmic value for sound</returns>
    public float VolumeLinearToLog(float linear)
    {
        if (linear<=0.001)
        {
            return -80f;
        }
        else
        {
            float temp = Mathf.Clamp(Mathf.Log10(linear) * 20, -80f, 20f);
            return temp;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
    }
}