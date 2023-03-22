using System.Collections.Generic;
using UnityEngine;

public class PlayerTutorial : MonoBehaviour
{
    public bool essenceTutorialCompleted = false;
    public bool shardTutorialCompleted = false;
    public bool injectionTutorialCompleted = false;
    public bool injectionBarTutorialCompleted = false;
    public bool movementTutorialCompleted = false;

    public const string SHARD_TUTORIAL = "SHARD_TUTORIAL";
    public const string ESSENCE_TUTORIAL = "ESSENCE_TUTORIAL";
    public const string INJECTION_TUTORIAL = "INJECTION_TUTORIAL";
    public const string INJECTION_BAR_TUTORIAL = "INJECTION_BAR_TUTORIAL";
    public const string MOVEMENT_TUTORIAL = "MOVEMENT_TUTORIAL";

    public GameObject ShardTutorialCanvas;
    public GameObject InjectionTutorialCanvas;
    public GameObject InjectionBarTutorialCanvas;
    public GameObject MovementLeftCanvas;
    public GameObject MovementRightCanvas;
    public GameObject MovementJumpCanvas;
    public GameObject essenceCanvas;

    public GameObject movementRightTrigger;
    public GameObject movementLeftTrigger;
    public GameObject movementJumpTrigger;
    public List<GameObject> hollowTreeArea;

    private void Awake()
    {
        if (!PlayerPrefs.HasKey(SHARD_TUTORIAL))
        {
            PlayerPrefs.SetInt(SHARD_TUTORIAL, 0);
        }
        if (!PlayerPrefs.HasKey(INJECTION_TUTORIAL))
        {
            PlayerPrefs.SetInt(INJECTION_TUTORIAL, 0);
        }
        if (!PlayerPrefs.HasKey(INJECTION_BAR_TUTORIAL))
        {
            PlayerPrefs.SetInt(INJECTION_BAR_TUTORIAL, 0);
        }
        if (!PlayerPrefs.HasKey(MOVEMENT_TUTORIAL))
        {
            PlayerPrefs.SetInt(MOVEMENT_TUTORIAL, 0);
        }
        if (!PlayerPrefs.HasKey(ESSENCE_TUTORIAL))
        {
            PlayerPrefs.SetInt(ESSENCE_TUTORIAL, 0);
        }

        PlayerPrefs.Save();
        essenceTutorialCompleted = PlayerPrefs.GetInt(ESSENCE_TUTORIAL, 0) == 0 ? false : true;
        shardTutorialCompleted = PlayerPrefs.GetInt(SHARD_TUTORIAL, 0) == 0 ? false : true;
        essenceTutorialCompleted = PlayerPrefs.GetInt(ESSENCE_TUTORIAL, 0) == 0 ? false : true;
        movementTutorialCompleted = PlayerPrefs.GetInt(MOVEMENT_TUTORIAL, 0) == 0 ? false : true;
        injectionTutorialCompleted = PlayerPrefs.GetInt(INJECTION_TUTORIAL, 0) == 0 ? false : true;
        injectionBarTutorialCompleted = PlayerPrefs.GetInt(INJECTION_BAR_TUTORIAL, 0) == 0 ? false : true;

        if (movementTutorialCompleted)
        {
            movementLeftTrigger.SetActive(false);
            movementRightTrigger.SetActive(false);
            movementJumpTrigger.SetActive(false);
            hollowTreeArea.ForEach(h => h.SetActive(false));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.LunarShard) && !shardTutorialCompleted)
        {
            Time.timeScale = 0f;
            shardTutorialCompleted = true;
            ShardTutorialCanvas.SetActive(true);
            PlayerPrefs.SetInt(SHARD_TUTORIAL, 1);
            PlayerPrefs.Save();
        }
        if (other.CompareTag(Tags.Injection) && !injectionTutorialCompleted)
        {
            Time.timeScale = 0f;
            injectionTutorialCompleted = true;
            InjectionTutorialCanvas.SetActive(true);
            PlayerPrefs.SetInt(INJECTION_TUTORIAL, 1);
            PlayerPrefs.Save();
        }
    }

    public void ShowInjectionBarTutorial(bool value)
    {
        if (value && !injectionBarTutorialCompleted)
        {
            Time.timeScale = 0f;
            InjectionBarTutorialCanvas.SetActive(true);
            injectionBarTutorialCompleted = true;
            PlayerPrefs.SetInt(INJECTION_BAR_TUTORIAL, 1);
            PlayerPrefs.Save();
        }
    }

    public void MovementTutorialDone()
    {
        PlayerPrefs.SetInt(MOVEMENT_TUTORIAL, 1);
        PlayerPrefs.Save();
    }

    public void MovementPause()
    {
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        InjectionBar.GoldenPeriod += ShowInjectionBarTutorial;
    }

    private void OnDisable()
    {
        InjectionBar.GoldenPeriod -= ShowInjectionBarTutorial;
    }
}
