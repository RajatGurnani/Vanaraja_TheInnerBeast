using System.Collections;
using UnityEngine;

public class PlayerNearMiss : MonoBehaviour
{
    public PlayerMovement playerMovement;

    public static System.Action<bool> NearMiss;
    public float nearMissTimer = 0f;
    public float nearMissCountdown = 5f;
    public bool alreadyNearMiss = false;
    public float multiplier = 0f;

    private void OnTriggerEnter(Collider other)
    {
        if (playerMovement.isWolf)
        {
            if (other.CompareTag(Tags.NearMiss))
            {
                multiplier += 0.5f;
                NearMiss?.Invoke(true);
                nearMissTimer = nearMissCountdown;
                if (!alreadyNearMiss)
                {
                    alreadyNearMiss = true;
                    StartCoroutine(nameof(Timer));
                }
            }
        }
    }

    private void Update()
    {
        if (alreadyNearMiss)
        {
            nearMissTimer -= Time.deltaTime;
            nearMissTimer = Mathf.Clamp(nearMissTimer, 0f, nearMissCountdown);
        }
    }

    IEnumerator Timer()
    {
        yield return new WaitUntil(() => nearMissTimer <= 0);
        alreadyNearMiss = false;
        multiplier = 0f;
        NearMiss?.Invoke(false);
    }
}
