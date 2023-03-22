using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowFps : MonoBehaviour
{
    public TMP_Text fpsText;
    public Queue<float> fpsValues = new Queue<float>();
    public float count = 0f;
    public int samples = 10;

    private void Update()
    {
        One();
        //Another();
    }

    public void One()
    {
        float value = 1 / Time.deltaTime;
        count += value;
        fpsValues.Enqueue(value);
        if (fpsValues.Count > samples)
        {
            count -= fpsValues.Dequeue();
        }
        fpsText.text = $"Fps: {count / fpsValues.Count:0}";
    }

    public void Another()
    {
        fpsText.text = $"Fps: {1 / Time.deltaTime:0}";
    }
}
