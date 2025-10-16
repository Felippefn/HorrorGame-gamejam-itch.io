using UnityEngine;
using System.Collections;

public class FirstSequenceController : MonoBehaviour
{

    [Header("References Stage 1")]
    public TVController tv;
    public NPCController tech;
    public WaypointPath pathToTV;
    public WaypointPath pathToExit;
    public float tvGlitchDelay = 5f;

    private bool serviceCalled;
    private bool fixedDone;


    [Header("References Stage 1.1")]
    public SitController sitController;
    public BoxCollider tvSpeakTrigger;
    public DoorHandleController door;


    [Header("References Stage 2")]
    public ScaryNeighborTrigger scaryNeighborTrigger;

    
    [Header("Stage Behavior")]
    private bool _stageSequenceDone;


    void Awake()
    {
        scaryNeighborTrigger.enabled = false;
        //sitController.enabled = false;
        door.canBeInteracted = false;
    }

    void Start()
    { 
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
        yield return new WaitForSeconds(4f);
        door.isOpen = true;
        yield return StartCoroutine(tech.DoServiceWithPaths(pathToTV, pathToExit));
        door.isOpen = false;

        door.canBeInteracted = false;

        //yield return new WaitUntil(() => fixedDone);
        print("tv arruma " + tv.IsBroken);
        tv.SetBroken(fixedDone);

        tech.OnFixed -= onFixed;

        print("tv liga");
        tv.SetPower(true);

        yield return new WaitForSeconds(tvGlitchDelay);
        // tv.SpeakSequence();
    }

    IEnumerator SitDownAndWait()
    {
        yield return new WaitForSeconds(5f);

        // tv.SpeakSequence();
    }

    
    public bool StageSequenceDone => _stageSequenceDone;

    public void SetStageSequenceDone()
    {
        _stageSequenceDone = true;
    }
}
