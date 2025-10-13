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

    void Start() { tv.SetPower(false); }

    public void CallService()
    {
        if (serviceCalled) return;
        serviceCalled = true;
        StartCoroutine(ServiceFlow());
    }

    IEnumerator ServiceFlow()
    {
        // Ativa NPC se estiver desativado na cena
        if (!tech.gameObject.activeSelf) tech.gameObject.SetActive(true);

        bool fixedDone = false;
        tech.OnFixed += () => fixedDone = true;

        // Caminho até a TV e saída
        yield return StartCoroutine(tech.DoServiceWithPaths(pathToTV, pathToExit));

        if (!fixedDone) fixedDone = true;

        tv.SetPower(true);
        yield return new WaitForSeconds(tvGlitchDelay);
        tv.SpeakSequence();
    }
}
