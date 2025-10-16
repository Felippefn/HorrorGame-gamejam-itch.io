using UnityEngine;
using System.Collections;

public class TVController : MonoBehaviour
{
    // public Animator anim;
    // public AudioSource voice;
    // public AudioClip[] lines;
    public float delayBetweenLines = 1.2f;

    // Propriedade correta
    public bool IsBroken { get; private set; } = true; 


    public bool IsOn { get; private set; }

    public void SetBroken(bool broken)
    {
        IsBroken = broken;
        if (IsBroken && IsOn)
        {
            IsOn = false;
            print("TV quebrou e foi desligada.");
            // if (anim) anim.SetBool("On", false);
        }
    }

    public void SetPower(bool on)
    {
        if (on && IsBroken)
        {
            print("TV está quebrada e NÃO pode ligar.");
            IsOn = false;
            // if (anim) anim.SetBool("On", false);
            return;
        }

        IsOn = on;
        print("TV " + (on ? "ligada" : "desligada"));
        // if (anim) anim.SetBool("On", on);
    }

    public void SpeakSequence()
    {
        if (IsBroken)
        {
            print("TV está quebrada, não fala nada");
            return;
        }
        if (!IsOn)
        {
            print("TV está desligada, não fala nada");
            return;
        }

        StartCoroutine(PlayWarningLine());
    }
    public void ChangeChannel()
    {
        if (IsBroken)
        {
            print("TV está quebrada, não troca de canal");
            return;
        }
        if (!IsOn)
        {
            print("TV está desligada, não troca de canal");
            return;
        }

        print("Trocando de canal na TV");
    }

    IEnumerator PlayWarningLine()
    {
        print("WAIT");
        yield return new WaitForSeconds(1f);
        print("DO NOT OPEN THAT DOOR");
    }
}
