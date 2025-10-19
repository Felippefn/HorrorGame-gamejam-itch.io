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
    //public BoxCollider tvSpeakTrigger;
    public DoorHandleController door;
    public TVSpeakCollider tvCollider;
    private bool _sitStarted;
    private bool _readyToSpeak;
    private bool _tvSpeakDone;


    [Header("References Stage 2")]
    public ScaryNeighborTrigger scaryNeighborTrigger;


    [Header("Stage Behavior")]
    private bool _stageSequenceDone;
    private bool _firstStageDone;


    void Awake()
    {
        scaryNeighborTrigger.enabled = false;
        //sitController.enabled = false;
        door.canBeInteracted = false;
        _firstStageDone = false;
        _stageSequenceDone = false;
    }

    void Start()
    { 
        tv.SetBroken(true);
        tv.SetPower(false);
        if (tech.gameObject.activeSelf) tech.gameObject.SetActive(false);
    }

    // ADIÇÃO: Update com polling leve e barato (só booleans)
void Update()
{
    // 1) Quando o serviço acabou e o player sentou, dispara a espera uma única vez
    if (_firstStageDone && !_sitStarted && sitController != null && sitController.sitting)
    {
        _sitStarted = true;
        StartCoroutine(SitDownAndWaitWrapper());
    }

    // 2) Depois da espera, libera a TV pra falar quando o player encostar no trigger
    if (_readyToSpeak && !_tvSpeakDone)
    {
        // Reaproveita sua própria checagem (IsOn + !IsBroken + playerInTrigger)
        TVSpeakIfPlayerInTrigger();

        // Se as condições já estão válidas nesse frame, marcamos como concluído
        if (tvCollider != null && tvCollider.playerInTrigger && tv.IsOn && !tv.IsBroken)
        {
            _tvSpeakDone = true; // evita múltiplas execuções
        }
    }
}
    IEnumerator SitDownAndWaitWrapper()
    {
        yield return StartCoroutine(SitDownAndWait());
        _readyToSpeak = true;
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

    public void TVSpeakIfPlayerInTrigger()
    {
        if (tvCollider.playerInTrigger && tv.IsOn && !tv.IsBroken)
        {
            print("Player esta sendado, tv ligada e não quebrada");
            print("Player está no trigger da TV");
            tv.SpeakSequence();
        }
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
        print("first phase is done");
        _firstStageDone = true;
        // tv.SpeakSequence();
    }

    IEnumerator SitDownAndWait()
    {
        yield return new WaitForSeconds(5f);

        //Knock knock
        print("Bate na porta");
        yield return new WaitForSeconds(2f);

        //Knock knock knock
        print("Bate mais forte na porta");
        yield return new WaitForSeconds(2f);

        // tv.SpeakSequence();
    }

    
    public bool StageSequenceDone => _stageSequenceDone;

    public void SetStageSequenceDone()
    {
        _stageSequenceDone = true;
    }
}
