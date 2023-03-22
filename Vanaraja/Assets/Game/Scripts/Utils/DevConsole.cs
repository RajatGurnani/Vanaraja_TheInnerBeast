using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DevConsole : MonoBehaviour
{
    public static DevConsole Instance;

    public TMP_Text timeScaleValueText;
    public bool showCanvas = false;
    public GameObject fpsCounter;
    public GameObject timeScaler;
    public GameObject toggleCanvas;
    public Slider timeSlider;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    public void ToggleCanvas() => toggleCanvas.SetActive(!toggleCanvas.activeSelf);

    public void ToggleTimeScaler() => timeScaler.SetActive(!timeScaler.activeSelf);

    public void ToggleFrameRate() => fpsCounter.SetActive(!fpsCounter.activeSelf);

    public void ChangeTimeValue()
    {
        timeScaleValueText.text = $" TimeScale: {timeSlider.value.ToString("F2")}";
        Time.timeScale = timeSlider.value;
    }
}