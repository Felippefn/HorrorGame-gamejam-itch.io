using UnityEngine;
using System.Collections;

public class TVController : MonoBehaviour
{
    //public Animator anim;               // opcional: anima shader/ligar
    //public AudioSource voice;           // arrasta um AudioSource
    //public AudioClip[] lines;           // falas da TV
    public float delayBetweenLines = 1.2f;

    public void SetPower(bool on)
    {
        Debug.Log("TV " + (on ? "ligada" : "desligada"));
        //if (anim) anim.SetBool("On", on);
        // também pode ligar/desligar emissive, VFX, etc.
    }

    public void SpeakSequence()
    {
        StartCoroutine(PlayLines());
    }

    IEnumerator PlayLines()
    {
        // foreach (var clip in lines)
        // {
        //     voice.clip = clip;
        //     voice.Play();
        //     yield return new WaitForSeconds(clip.length + delayBetweenLines);
        // }
        Debug.Log("TV começando a falar a sequência");
        yield return new WaitForSeconds(1f);
        Debug.Log("TV falou a sequência");
        // aqui você pode disparar a virada da TV (luz apaga, porta tranca, jumpscare etc.)
    }
}
