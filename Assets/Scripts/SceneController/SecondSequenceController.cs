using System.Collections;
using UnityEngine;

public class SecondSequenceController : MonoBehaviour
{
    [Header("Refs")]
    public TVController tv;                 // a mesma TV da fase 1
    public GameObject tvRoot;               // objeto com Interactable/Collider da TV (para travar interação)
    public LookAtWatcher lookWatcher;       // arraste o watcher apontando pro vaso/spot
    public GameObject enemyBehind;          // inimigo que só aparecerá via espelho
    public AudioSource flushSource;         // áudio da descarga
    public AudioSource bgCreepySource;            // áudio de fundo (pode abaixar volume, se quiser)
    public AudioSource jumpScareSource;       // áudio do jump scare

    public DoorHandleController doorBathroomHandle; // referência à porta (se quiser travar)

    
    bool flushedToilet = false;
    //bool bathroomDoorInitiallyState = false;
    public bool EnemyRevealed { get; private set; }

    [Header("Timing")]
    public float flushDelayMin = 6f;
    public float flushDelayMax = 12f;

    public bool StageSequenceDone { get; private set; }

    Interactable tvInteractable;
    Collider tvCollider;

    void OnEnable()
    {
        StageSequenceDone = false;
        EnemyRevealed = false;
        // 1) Travar interação com a TV
        if (tvRoot)
        {
            tvInteractable = tvRoot.GetComponent<Interactable>();
            tvCollider = tvRoot.GetComponent<Collider>();
            if (tvInteractable) tvInteractable.enabled = false;
            if (tvCollider) tvCollider.enabled = false;
        }
        if (tv) tv.SetPower(false); // garante que não “liga” mais

        // 2) Preparar o reveal
        if (enemyBehind) enemyBehind.SetActive(false);
        if (lookWatcher)
        {
            lookWatcher.ResetWatch();
            lookWatcher.onLook.RemoveAllListeners();
            lookWatcher.onLook.AddListener(OnPlayerLooked);
            if (!lookWatcher.cam) lookWatcher.cam = Camera.main;
        }

        // 3) Começar a sequência
        StartCoroutine(RunSecondStage());
    }

    void Update()
    {
        if (flushedToilet && doorBathroomHandle.playerInteracted)
        {
           if (bgCreepySource.isPlaying) StartCoroutine(FadeOutSound(bgCreepySource, 3f)); // 3 segundos de fade
            print("Player abriu a porta do banheiro antes de olhar pro espelho.");
        }
    }

    IEnumerator FadeOutSound(AudioSource source, float fadeTime = 2f)
    {
        if (source == null) yield break;

        float startVolume = source.volume;

        while (source.volume > 0f)
        {
            source.volume -= startVolume * Time.deltaTime / fadeTime;
            yield return null;
        }

        source.Stop();
        source.volume = startVolume; // reseta volume original pra próximas vezes
    }


    IEnumerator RunSecondStage()
    {
        // Delay aleatório antes da descarga
        float t = Random.Range(flushDelayMin, flushDelayMax);
        yield return new WaitForSeconds(t);

        // Descarga toca do nada
        flushedToilet = true;
        print("Toquei a descarga!");
        if (flushSource && flushSource.clip)
            flushSource.PlayOneShot(flushSource.clip);

        yield return new WaitForSeconds(1f);

        bgCreepySource.PlayOneShot(bgCreepySource.clip);
        // Agora só esperamos o player olhar (LookAtWatcher chama OnPlayerLooked)
        // Se quiser timeout para forçar o susto mesmo sem olhar, descomente:
        // yield return new WaitForSeconds(10f);
        // if (!StageSequenceDone) OnPlayerLooked();
    }

    public void OnPlayerLooked()
    {
        
        print("Player olhou para o espelho!");
        print("Revelando inimigo...");
        jumpScareSource.PlayOneShot(jumpScareSource.clip);
        // Ativa o inimigo (você cuida do espelho/posição)
       // if (enemyBehind) enemyBehind.SetActive(true);
        if (enemyBehind) enemyBehind.SetActive(true);
        EnemyRevealed = true;        // <-- marca que o inimigo já apareceu
        
        
        // Finaliza a fase 2
        StageSequenceDone = true;

        // Mantém TV travada
        if (tvRoot)
        {
            if (tvInteractable) tvInteractable.enabled = false;
            if (tvCollider) tvCollider.enabled = false;
        }
    }

    void OnDisable()
    {
        // limpeza
        if (lookWatcher) lookWatcher.onLook.RemoveListener(OnPlayerLooked);
    }
}
