using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource mainMenuSource;
    public AudioSource musicSource;

    public float fadeInTime;
    IEnumerator Transition()
    {
        while (mainMenuSource.volume > 0 || musicSource.volume < 1)
        {
            musicSource.volume = Mathf.Clamp01(musicSource.volume += Time.deltaTime * (1 / fadeInTime));
            mainMenuSource.volume = Mathf.Clamp01(mainMenuSource.volume -= Time.deltaTime * (1 / fadeInTime));
            //yield return new WaitForSeconds(0.5f);
            yield return new WaitForEndOfFrame();
        }
    }

    public void StartTransition()
    {
        StartCoroutine(nameof(Transition));
    }

    private void OnEnable()
    {
        GameManager.GameStarted += StartTransition;
    }

    private void OnDisable()
    {
        GameManager.GameStarted -= StartTransition;
    }
}
