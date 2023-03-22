using UnityEngine;

/// <summary>
/// All audio settings related to player
/// </summary>
public class PlayerAudio : MonoBehaviour
{
    public AudioSource preWolfSource;
    public AudioSource footstepSource;

    public AudioClip humanFootstep;
    public AudioClip wolfFootstep;

    bool isWolf = false;

    public void AboutToTurnIntoWolf()
    {
        preWolfSource.Play();
    }

    /// <summary>
    /// Function to be called using an animation event
    /// when the player is about to/makes a footstep
    /// </summary>
    public void PlayFootstep()
    {
        if (isWolf)
        {
            footstepSource.PlayOneShot(wolfFootstep);
        }
        else
        {
            footstepSource.PlayOneShot(humanFootstep);
        }
    }

    public void CurrentState(bool value)
    {
        isWolf = value;
    }

    private void OnEnable()
    {
        SwitchForm.SwitchStates += CurrentState;
        SwitchForm.AboutToTurnIntoWolf += AboutToTurnIntoWolf;
    }

    private void OnDisable()
    {
        SwitchForm.SwitchStates -= CurrentState;
        SwitchForm.AboutToTurnIntoWolf -= AboutToTurnIntoWolf;
    }
}
