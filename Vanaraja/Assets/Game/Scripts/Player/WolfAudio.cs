using UnityEngine;

public class WolfAudio : MonoBehaviour
{
    public AudioSource footstepSource;

    private void OnEnable()
    {
        footstepSource.Play();
    }

    private void OnDisable()
    {
        footstepSource.Pause();
    }
}
