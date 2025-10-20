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

    [Header("Timing")]
    public float flushDelayMin = 6f;
    public float flushDelayMax = 12f;

    public bool StageSequenceDone { get; private set; }

    Interactable tvInteractable;
    Collider tvCollider;

    void OnEnable()
    {
        StageSequenceDone = false;

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

    IEnumerator RunSecondStage()
    {
        // Delay aleatório antes da descarga
        float t = Random.Range(flushDelayMin, flushDelayMax);
        yield return new WaitForSeconds(t);

        // Descarga toca do nada
        if (flushSource && flushSource.clip)
            flushSource.PlayOneShot(flushSource.clip);

        // Agora só esperamos o player olhar (LookAtWatcher chama OnPlayerLooked)
        // Se quiser timeout para forçar o susto mesmo sem olhar, descomente:
        // yield return new WaitForSeconds(10f);
        // if (!StageSequenceDone) OnPlayerLooked();
    }

    void OnPlayerLooked()
    {
        // Ativa o inimigo (você cuida do espelho/posição)
        if (enemyBehind) enemyBehind.SetActive(true);

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
