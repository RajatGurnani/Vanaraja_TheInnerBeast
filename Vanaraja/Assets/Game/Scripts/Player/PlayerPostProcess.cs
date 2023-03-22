using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerPostProcess : MonoBehaviour
{
    public ScriptableRendererFeature highlightOpaque;
    public ScriptableRendererFeature highlightTransparent;
    public Volume humanVolume;
    public Volume wolfVolume;
    public ParticleSystem speedLinesParticleSystem;
    public float blendTime = 2f;

    private void Awake()
    {
        highlightOpaque.SetActive(false);
        highlightTransparent.SetActive(false);
        humanVolume.weight = 1f;
        wolfVolume.weight = 0f;
    }

    private void Start()
    {
        ParticleSystem.MainModule temp = speedLinesParticleSystem.main;
        temp.loop = false;
        speedLinesParticleSystem.Stop();
    }

    public void SwitchToWolf(bool value)
    {
        highlightOpaque.SetActive(value);
        highlightTransparent.SetActive(value);
        StartCoroutine(ChangeEffects(value));
    }

    /// <summary>
    /// Changes from Human ->Wolf if value is "true" and  vice versa
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    IEnumerator ChangeEffects(bool value)
    {
        if (value)
        {
            ParticleSystem.MainModule temp = speedLinesParticleSystem.main;
            temp.loop = true;
            speedLinesParticleSystem.Play();
            while (humanVolume.weight > 0f || wolfVolume.weight < 1f)
            {
                humanVolume.weight = Mathf.Clamp01(humanVolume.weight -= Time.deltaTime / blendTime);
                wolfVolume.weight = Mathf.Clamp01(wolfVolume.weight += Time.deltaTime / blendTime);
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            ParticleSystem.MainModule temp =speedLinesParticleSystem.main;
            temp.loop = false;
            while (humanVolume.weight < 1f || wolfVolume.weight > 0f)
            {
                humanVolume.weight = Mathf.Clamp01(humanVolume.weight += Time.deltaTime / blendTime);
                wolfVolume.weight = Mathf.Clamp01(wolfVolume.weight -= Time.deltaTime / blendTime);
                yield return new WaitForEndOfFrame();
            }
            speedLinesParticleSystem.Stop();
        }
    }

    private void OnEnable()
    {
        SwitchForm.SwitchStates += SwitchToWolf;
    }

    private void OnDisable()
    {
        SwitchForm.SwitchStates -= SwitchToWolf;
    }
}
