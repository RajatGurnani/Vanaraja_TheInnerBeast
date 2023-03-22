using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.PlayerLoop;

public class Tutorial : MonoBehaviour
{
    public bool tutorialDone = false;
    public const string TUTORIAL = "TUTORIAL";

    public List<GameObject> tutorialObjects = new List<GameObject>();
    public PlayableDirector playableScene;
    public PlayerMovement playerMovement;
    float speed;
    public float factor;

    private void Start()
    {
        playerMovement = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerMovement>();
    }

    public void CheckForTutorial()
    {
        if (!PlayerPrefs.HasKey(TUTORIAL))
        {
            PlayerPrefs.SetInt(TUTORIAL, 0);
            PlayerPrefs.Save();
        }
        tutorialDone = PlayerPrefs.GetInt(TUTORIAL, 0) == 1;
        tutorialObjects.ForEach(x => x.SetActive(!tutorialDone));
    }
    public void ReplayTutorial()
    {
        PlayerPrefs.SetInt(TUTORIAL, 0);
        PlayerPrefs.Save();
        CheckForTutorial();
    }

    private void Awake()
    {
        CheckForTutorial();
        tutorialObjects.ForEach(x => x.SetActive(!tutorialDone));
    }

    public void OnBridgeEnter()
    {
        speed = playerMovement.forwardSpeed;
        playableScene.Play();
        StartCoroutine(nameof(Slow));
    }

    public void OnBridgeStay()
    {

    }

    public void OnBridgeExit()
    {
        //playableScene.Stop();
        //StopCoroutine(nameof(Slow));
        Invoke(nameof(Stop), 3f);
    }

    public void Stop()
    {
        StopCoroutine(nameof(Slow));
    }

    public void Update()
    {
    }

    IEnumerator Slow()
    {
        while (true)
        {
            playerMovement.forwardSpeed = speed * factor;
            yield return null;
        }
    }

    public void ShowMovement()
    {
        Time.timeScale = 0f;
    }

    public void ShowShardAndBar()
    {
        Time.timeScale = 0f;
        PlayerPrefs.SetInt(TUTORIAL, 1);
        PlayerPrefs.Save();

    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }
}
