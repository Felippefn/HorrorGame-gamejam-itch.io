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

    public AudioSource knockSource;
    public AudioSource knockSource2;
    public AudioSource knockSource3;

    public SitController sitController;
    //public BoxCollider tvSpeakTrigger;
    public DoorHandleController door;
    public TVSpeakCollider tvCollider;
    private bool _sitStarted;
    private bool _readyToSpeak;
    private bool _tvSpeakDone;
    private bool _speakForced;
    private Coroutine _knockLoop;
    private Coroutine _speakWhenReady;





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
    if (_readyToSpeak && !_tvSpeakDone)
    {
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
        if (_tvSpeakDone) return;

        if (tvCollider != null && tvCollider.playerInTrigger)
        {
            // Força a fala assim que as condições da TV permitirem
            ForceSpeakNow();
        }
    }

    // Chame isso para "furar fila" e fazer a TV falar assim que puder
    void ForceSpeakNow()
    {
        if (_tvSpeakDone) return;
        _speakForced = true;

        // Para o loop de batidas, se estiver tocando
        if (_knockLoop != null)
        {
            StopCoroutine(_knockLoop);
            _knockLoop = null;
        }

        // Se já existe uma espera de fala, não duplique
        if (_speakWhenReady == null)
            _speakWhenReady = StartCoroutine(SpeakWhenReady());
    }

    IEnumerator SpeakWhenReady()
    {
        // Espera até a TV estar ligada e não quebrada
        yield return new WaitUntil(() => tv != null && tv.IsOn && !tv.IsBroken);

        tv.SpeakSequence();
        _tvSpeakDone = true;
        _speakWhenReady = null;
    }


    IEnumerator ServiceFlow()
    {
        
        tech.gameObject.SetActive(true);
        if (!tech.gameObject.activeSelf) tech.gameObject.SetActive(true);

        System.Action onFixed = () => fixedDone = true;
        tech.OnFixed += onFixed;
        yield return new WaitForSeconds(4f);
        tv.SetBroken(fixedDone);
        yield return StartCoroutine(tech.DoServiceWithPaths(pathToTV, pathToExit));
        door.canBeInteracted = false;

        //yield return new WaitUntil(() => fixedDone);
        //tv.SetBroken(fixedDone);

        tech.OnFixed -= onFixed;

        tv.SetPower(true);

        yield return new WaitForSeconds(tvGlitchDelay);
        print("first phase is done");
        _firstStageDone = true;
        // tv.SpeakSequence();
    }

    IEnumerator SitDownAndWait()
    {
        // Pequena espera antes de começar a incomodar
        yield return new WaitForSeconds(5f);

        // Inicia o loop de batidas (se já não estiver rodando)
        if (_knockLoop == null)
            _knockLoop = StartCoroutine(KnockLoop());

    }

    IEnumerator KnockLoop()
    {
        // Enquanto o player estiver sentado e a TV ainda não falou (ou não foi forçada)
        while (sitController != null && sitController.sitting && !_tvSpeakDone && !_speakForced)
        {
            if (door != null && door.playerInteracted) break;
            // Knock 1
            if (knockSource && knockSource.clip) knockSource.PlayOneShot(knockSource.clip);
            yield return new WaitForSeconds(2f);

            // Knock 2 (mais forte)
            if (knockSource2 && knockSource2.clip) knockSource2.PlayOneShot(knockSource2.clip);
            yield return new WaitForSeconds(2f);

            // Knock 3 (super forte)
            if (knockSource3 && knockSource3.clip) knockSource3.PlayOneShot(knockSource3.clip);
            yield return new WaitForSeconds(2f);
            // Pausa antes de repetir o padrão
            yield return new WaitForSeconds(2f);

            // Se o jogador levantou, o while vai terminar naturalmente na próxima verificação
            // Se quiser quebrar imediatamente quando levantar:
            if (sitController != null && !sitController.sitting) break;
        }

        _knockLoop = null;
    }

    
    public bool StageSequenceDone => _stageSequenceDone;

    public void SetStageSequenceDone()
    {
        _stageSequenceDone = true;
    }
}
