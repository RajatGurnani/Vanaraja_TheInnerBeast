using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

/// <summary>
/// It will contain the option of changing the 
/// - audio levels
/// - quality
/// - vibration
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    private string privacyUrl = "https://sites.google.com/view/nerdyquest/home";
    private string creditsUrl = "https://sites.google.com/view/nerdyquestcredits/home";

    [Header("Sound")]
    public Slider musicSlider;
    public Slider sfxSlider;
    AudioManager audioManager;

    public Slider masterSlider;
    public Slider renderScaleSlider;

    [Header("Control Schemes")]
    public ToggleGroup controlsToggle;
    public Toggle tiltToggle;
    public Toggle leftRightToggle;
    public const string USING_TILT_CONTROLS = "USING_TILT_CONTROLS";
    public bool usingTilt = false;

    public Toggle vibrationToggle;

    [Header("Quality")]
    public UniversalRenderPipelineAsset urpAsset;
    public Toggle qualityToggle;
    public TMP_Text qualityText;
    public const string SCALING = "SCALING";
    public int scale = 1;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        Init();
        LoadControlScheme();
        LoadRenderScale();
    }

    public void LoadControlScheme()
    {
        if (!PlayerPrefs.HasKey(USING_TILT_CONTROLS))
        {
            PlayerPrefs.SetInt(USING_TILT_CONTROLS, 0);
            PlayerPrefs.Save();
        }
        usingTilt = PlayerPrefs.GetInt(USING_TILT_CONTROLS, 0) == 1;
        tiltToggle.SetIsOnWithoutNotify(usingTilt);
        leftRightToggle.SetIsOnWithoutNotify(!usingTilt);
    }

    public void LoadRenderScale()
    {
        if (!PlayerPrefs.HasKey(SCALING))
        {
            PlayerPrefs.SetInt(SCALING, 0);
            PlayerPrefs.Save();
        }
        scale = PlayerPrefs.GetInt(SCALING, 0);
        ChangeRenderScale();
    }

    public void ChangeRenderScale(int value = 0)
    {
        scale = (int)Mathf.Repeat(scale + value, 3);
        switch (scale)
        {
            case 0:
                qualityText.text = "GRAPHICS: HIGH";
                urpAsset.renderScale = 1f;
                //ScalableBufferManager.ResizeBuffers(1f, 1f);
                break;
            case 1:
                qualityText.text = "GRAPHICS: MEDIUM";
                urpAsset.renderScale = 0.87f;
                //ScalableBufferManager.ResizeBuffers(0.87f, 0.87f);
                break;
            case 2:
                qualityText.text = "GRAPHICS: LOW";
                urpAsset.renderScale = 0.75f;
                //ScalableBufferManager.ResizeBuffers(0.75f, 0.75f);
                break;
            default:
                qualityText.text = "GRAPHICS: HIGH";
                urpAsset.renderScale = 1f;
                //ScalableBufferManager.ResizeBuffers(1f, 1f);
                break;
        }
        PlayerPrefs.SetInt(SCALING, scale);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Initializing the slider values without
    /// triggering their onValueChangedFuntion
    /// </summary>
    public void Init()
    {
        sfxSlider.SetValueWithoutNotify(audioManager.sfx * 10);
        musicSlider.SetValueWithoutNotify(audioManager.music * 10);
        //masterSlider.SetValueWithoutNotify(audioManager.master);
    }

    public void SwitchControls(Toggle toggle)
    {
        if (toggle == tiltToggle)
        {
            usingTilt = toggle.isOn;
        }
        Debug.Log("usingTilt- " + usingTilt);
        PlayerPrefs.SetInt(USING_TILT_CONTROLS, usingTilt ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void OpenCreditsUrl() => Application.OpenURL(creditsUrl);
    public void OpenPrivacyPolicy() => Application.OpenURL(privacyUrl);
    public void ChangeSFXVolume()
    {
        UiSFXManager.Instance.PlaySFX(UiSFXManager.SFXType.SliderTick);
        audioManager.SetSFXVolume(sfxSlider.value / 10);
    }

    //public void ChangeMasterVolume() => audioManager.SetMasterVolume(masterSlider.value);
    public void ChangeMusicVolume()
    {
        UiSFXManager.Instance.PlaySFX(UiSFXManager.SFXType.SliderTick);
        audioManager.SetMusicVolume(musicSlider.value / 10);
    }
}
