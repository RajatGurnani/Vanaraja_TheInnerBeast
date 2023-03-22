using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MoonUI : MonoBehaviour
{
    public Gradient moonGradient;
    public Gradient gradient;
    public GradientColorKey[] colorKeys = new GradientColorKey[4];
    public GradientAlphaKey[] alphaKeys;

    public Color startColor;
    public Color transitionColor;
    public Color wolfColor;
    public Color endColor;
    public Image moonImage;
    public Image wolfImage;

    public AnimationCurve wolfAnim;
    public Vector2 imageSize;
    public SwitchForm switchForm;

    [Header("New UI")]
    public float changeTime = 0.5f;
    public Image moonGlow;
    public Image moonRadialBar;
    public Color32 humanStateColor;
    public Color32 wolfStateColor;

    private void Start()
    {
        switchForm = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<SwitchForm>();
        imageSize = wolfImage.rectTransform.sizeDelta;
        SetGradient();
    }

    private void Update()
    {
        //ChangeMoon(switchForm.normalizedValue, switchForm.normalizedTransformationValue, switchForm.normalizedWolfTime);
        NewChangeMoon();
    }

    public void ChangeMoon(float changeValue, float transitionValue, float wolfValue)
    {
        moonImage.color = moonGradient.Evaluate(changeValue);
        if (changeValue > transitionValue)
        {
            float val = Mathf.Clamp01((changeValue - transitionValue) / (wolfValue - transitionValue));
            wolfImage.color = Color.Lerp(Color.clear, Color.white, val);
            //wolfImage.rectTransform.anchoredPosition = (Mathf.Lerp(0, 1, val) - 1) * imageSize;
        }
        else
        {
            wolfImage.color = Color.clear;
            //wolfImage.rectTransform.anchoredPosition = -1 * imageSize;
        }
    }

    public void NewChangeMoon()
    {
        if (!switchForm.isWolf)
        {
            moonRadialBar.fillClockwise = false;
            moonRadialBar.fillAmount = 1 - switchForm.timer / switchForm.humanTime;
        }
        else
        {
            moonRadialBar.fillClockwise = true;
            moonRadialBar.fillAmount = 1 - (switchForm.timer - switchForm.humanTime) / switchForm.wolfTime;
        }
    }

    public void ChangeState(bool value)
    {
        moonRadialBar.DOFillAmount(1, changeTime);
        if (value)
        {
            wolfImage.DOFade(0.5f, changeTime);
            moonImage.DOColor(wolfStateColor, changeTime);
            moonGlow.DOColor(wolfStateColor, changeTime);
            moonRadialBar.DOColor(wolfStateColor, changeTime);
        }
        else
        {
            wolfImage.DOFade(0, changeTime);
            moonImage.DOColor(humanStateColor, changeTime);
            moonGlow.DOColor(humanStateColor, changeTime);
            moonRadialBar.DOColor(humanStateColor, changeTime);
        }
    }

    public void SetGradient()
    {
        gradient = moonGradient;
        colorKeys = moonGradient.colorKeys;
        colorKeys.SetValue(new GradientColorKey(startColor, 0f), 0);
        colorKeys.SetValue(new GradientColorKey(transitionColor, switchForm.normalizedValue), 1);
        colorKeys.SetValue(new GradientColorKey(wolfColor, switchForm.normalizedWolfTime), 2);
        colorKeys.SetValue(new GradientColorKey(endColor, 1f), 3);
        moonGradient.SetKeys(colorKeys, moonGradient.alphaKeys);
    }

    private void OnEnable()
    {
        SwitchForm.SwitchStates += ChangeState;
    }

    private void OnDisable()
    {
        SwitchForm.SwitchStates -= ChangeState;
    }
}
