using UnityEngine;
using System.Collections;

public class TVController : MonoBehaviour
{
    // public Animator anim;
    // public AudioSource voice;
    // public AudioClip[] lines;
    public float delayBetweenLines = 1.2f;

    // Propriedade correta
    public bool IsBroken { get; private set; } = true; // começa quebrada? ajuste se quiser

    // Opcional: estado de ligada/desligada (se precisar)
    public bool IsOn { get; private set; }

    // Método para alterar estado de quebrada
    public void SetBroken(bool broken)
    {
        IsBroken = broken;
        if (IsBroken && IsOn)
        {
            // Se quebrou enquanto ligada, desliga
            IsOn = false;
            Debug.Log("TV quebrou e foi desligada.");
            // if (anim) anim.SetBool("On", false);
        }
    }

    public void SetPower(bool on)
    {
        if (on && IsBroken)
        {
            Debug.Log("TV está quebrada e NÃO pode ligar.");
            IsOn = false;
            // if (anim) anim.SetBool("On", false);
            return;
        }

        IsOn = on;
        Debug.Log("TV " + (on ? "ligada" : "desligada"));
        // if (anim) anim.SetBool("On", on);
    }

    public void SpeakSequence()
    {
        if (IsBroken)
        {
            Debug.Log("TV está quebrada, não fala nada");
            return;
        }
        if (!IsOn)
        {
            Debug.Log("TV está desligada, não fala nada");
            return;
        }

        StartCoroutine(PlayLines());
    }

    IEnumerator PlayLines()
    {
        Debug.Log("TV começando a falar a sequência");
        yield return new WaitForSeconds(1f);
        Debug.Log("TV falou a sequência");
    }
}
