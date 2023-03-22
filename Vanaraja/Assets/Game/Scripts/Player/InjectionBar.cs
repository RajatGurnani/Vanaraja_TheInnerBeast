using System.Collections;
using UnityEngine;

public class InjectionBar : MonoBehaviour
{
    Coroutine drainCoroutine;

    [Header("Injection Bar")]
    public float energy;
    public float injectionIncrement = 0.4f;
    public float injectionDrainPerSecond = 0.1f;
    public float invincibleDrain = 0.1f;

    public static bool goldenPeriod = false;
    public static System.Action<bool> GoldenPeriod;

    PlayerCollision playerCollision;
    public AudioSource injectionSource;
    public AudioSource invincibleSource;
    public AudioClip injectionClip;

    public bool continueRun = true;

    public void Awake()
    {
        goldenPeriod = false;
        playerCollision = GetComponent<PlayerCollision>();
    }

    IEnumerator InjectionMeterDrain()
    {
        while (true)
        {
            if (goldenPeriod)
            {
                energy -= Time.deltaTime * invincibleDrain;
            }
            else
            {
                energy -= Time.deltaTime * injectionDrainPerSecond;
            }
            energy = Mathf.Clamp01(energy);
            if (goldenPeriod && energy <= 0)
            {
                goldenPeriod = false;
                GoldenPeriod?.Invoke(false);
                invincibleSource.Play();
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void InjectionCollected()
    {
        injectionSource.PlayOneShot(injectionClip);
        energy += injectionIncrement;
        energy = Mathf.Clamp01(energy);
        if (energy >= 1f)
        {
            goldenPeriod = true;

            GoldenPeriod?.Invoke(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Injection) && !playerCollision.invincible)
        {
            InjectionCollected();
        }
    }

    public void GameStarted()
    {
        if (!continueRun)
        {
            goldenPeriod = true;
            energy = 1;
            GoldenPeriod?.Invoke(true);
        }
        continueRun = false;
        drainCoroutine = StartCoroutine(nameof(InjectionMeterDrain));
    }

    public void GameOver()
    {
        StopCoroutine(drainCoroutine);
    }

    private void OnEnable()
    {
        GameManager.GameOver += GameOver;
        GameManager.GameStarted += GameStarted;
    }

    private void OnDisable()
    {
        GameManager.GameOver -= GameOver;
        GameManager.GameStarted -= GameStarted;
    }
}
