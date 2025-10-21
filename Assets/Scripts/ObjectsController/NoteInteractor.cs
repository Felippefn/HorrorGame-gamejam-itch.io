using UnityEngine;

public class NoteInteractor : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip noteSound;
    public GameObject panelToOpen;
    

    public void PlayNoteSound()
    {
        if (audioSource != null && noteSound != null)
        {
            audioSource.PlayOneShot(noteSound);
        }
    }
}