using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SFXButton : MonoBehaviour
{
    UiSFXManager sfxSource;
    public UiSFXManager.SFXType type;

    Button button;
    RectTransform rectTransform;
    public Vector3 scaleIncrease = new Vector3(0.2f, 0.2f, 0.2f);
    public float scaleTime = 0.5f;

    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() => PlaySound());
    }

    private void Start()
    {
        sfxSource = UiSFXManager.Instance;
    }

    public void PlaySound()
    {
        DOTween.Kill(rectTransform, true);
        rectTransform.DOPunchScale(scaleIncrease, scaleTime, 0, 1).OnComplete(() => rectTransform.localScale = Vector3.one);
        sfxSource.PlaySFX(type);
    }
}
