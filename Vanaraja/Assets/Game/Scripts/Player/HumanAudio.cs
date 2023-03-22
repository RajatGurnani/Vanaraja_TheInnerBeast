using UnityEngine;

public class HumanAudio : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip footstepClip;

    public void OnFootstep()
    {
        footstepSource.PlayOneShot(footstepClip);
    }
}
