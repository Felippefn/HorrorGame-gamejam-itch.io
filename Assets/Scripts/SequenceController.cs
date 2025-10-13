using UnityEngine;
using System.Collections;

public class SequenceController : MonoBehaviour
{
    public TVController tv;
    public NPCController tech;
    //public AudioSource sfxKnock;     // som de batida na porta
    public float tvGlitchDelay = 5f; // tempo após o técnico sair p/ a TV “acordar”

    bool serviceCalled;

    // 1) Ao entrar no quarto
    void Start()
    {
        // TV começa quebrada
        tv.SetPower(false);
    }

    // 2) Vem do Interactable do telefone/campainha
    public void CallService()
    {
        Debug.Log("CallService CALLING");
        Debug.Log("CallService serviceCalled: " + serviceCalled);
        if (serviceCalled) return;
        serviceCalled = true;
        StartCoroutine(ServiceFlow());
    }

    IEnumerator ServiceFlow()
    {
        // Batidas na porta (efeito)
        //if (sfxKnock) { sfxKnock.Play(); yield return new WaitForSeconds(sfxKnock.clip.length); }

        // Técnico chega, conserta e vai embora
        bool fixedDone = false;
        tech.OnFixed += () => fixedDone = true;
        yield return StartCoroutine(tech.DoService());

        // Garante que sinalizou consertado
        if (!fixedDone) fixedDone = true;

        // 3) Liga a TV quando consertada (mas muda depois…)
        tv.SetPower(true);

        // 4) Após um delay, TV começa a falar (virada)
        yield return new WaitForSeconds(tvGlitchDelay);

        // Efeito de glitch opcional antes das falas
        // tv.SetPower(false); yield return new WaitForSeconds(0.2f); tv.SetPower(true);

        tv.SpeakSequence();  // fala com o jogador
    }
}
