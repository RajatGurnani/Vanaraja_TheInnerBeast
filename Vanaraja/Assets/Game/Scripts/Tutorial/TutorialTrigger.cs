using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public UnityEvent enterEvent;
    public UnityEvent stayEvent;
    public UnityEvent exitEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            enterEvent?.Invoke();
        }
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.CompareTag(Tags.Player))
        {
            stayEvent?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            exitEvent?.Invoke();
        }
    }
}