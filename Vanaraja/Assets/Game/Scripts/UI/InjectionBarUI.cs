using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class InjectionBarUI : MonoBehaviour
{
    public float transitionValue = 1f;
    public Color32 invincibleColor;
    public Slider bar;
    public Image fill;
    public Image wolfImage;
    InjectionBar injectionBar;

    private void Start()
    {
        injectionBar = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<InjectionBar>();
    }

    private void Update()
    {
        bar.SetValueWithoutNotify(injectionBar.energy);
    }

    public void AddRemoveGlow(bool value)
    {
        fill.DOColor(value ? invincibleColor : Color.white, 2f);
        //Sequence sequence = DOTween.Sequence();
        //sequence.Append(wolfImage.DOColor(value ? Color.blue : Color.white, 2f));
        //sequence.Append(fill.DOColor(value ? Color.blue : Color.white, 2f));
        //sequence.Append(wolfImage.DOColor(value ? Color.blue : Color.white, 2f));
    }

    private void OnEnable() => InjectionBar.GoldenPeriod += AddRemoveGlow;
    private void OnDisable() => InjectionBar.GoldenPeriod -= AddRemoveGlow;
}
