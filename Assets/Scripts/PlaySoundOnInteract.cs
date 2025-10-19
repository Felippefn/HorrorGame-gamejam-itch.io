using UnityEngine;

public class PlaySoundOnInteract : MonoBehaviour
{
    public AudioSource audioSource;


    public void PlayInteractionSound()
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(audioSource.clip);
        }
    }
}