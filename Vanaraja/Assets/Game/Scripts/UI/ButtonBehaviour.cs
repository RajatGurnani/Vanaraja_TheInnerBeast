using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    public Button button;
    public UiSFXManager.SFXType sfxType;

    private void Awake() => button.onClick.AddListener(() => ButtonPress());
    public void ButtonPress() => UiSFXManager.Instance.PlaySFX(sfxType);
}
