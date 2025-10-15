using UnityEngine;
using System.Collections;

public class SequenceController : MonoBehaviour
{
    public TVController tv;
    public NPCController tech;
    public WaypointPath pathToTV;
    public WaypointPath pathToExit;
    public float tvGlitchDelay = 5f;

    bool serviceCalled;
    bool fixedDone = false;

    void Start() 
    { 
        // comece como quebrada se quiser (coerente com TV iniciar quebrada)
        tv.SetBroken(true);
        tv.SetPower(false);
        if (tech.gameObject.activeSelf) tech.gameObject.SetActive(false);
    }

    public void CallService()
    {
        if (serviceCalled) return;
        serviceCalled = true;
        StartCoroutine(ServiceFlow());
    }

    public void RadioMusic()
    {
        print("Tocando música no rádio");
    }

    IEnumerator ServiceFlow()
    {
        tech.gameObject.SetActive(true);
        if (!tech.gameObject.activeSelf) tech.gameObject.SetActive(true);

        
        System.Action onFixed = () => fixedDone = true;
        tech.OnFixed += onFixed;

        // Faz o serviço (anda até a TV e depois saída)
        yield return StartCoroutine(tech.DoServiceWithPaths(pathToTV, pathToExit));

        // estado da TV após o técnico tentar consertar
        print("tv arruma " + tv.IsBroken);
        tv.SetBroken(fixedDone);

        // desinscreve do evento (boa prática)
        tech.OnFixed -= onFixed;

        // Tenta ligar — SetPower vai bloquear se ainda estiver quebrada
        print("tv liga");
        tv.SetPower(true);

        yield return new WaitForSeconds(tvGlitchDelay);
        //tv.SpeakSequence();
    }
}
