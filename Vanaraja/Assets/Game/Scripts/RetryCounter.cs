using UnityEngine;

public class RetryCounter : MonoBehaviour
{
    public int count = 0;
    public int maxRetry = 2;

    public bool CanRetry() => count < maxRetry;
    public void IncrementRetries() => ++count;
}
