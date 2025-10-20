using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ScarySceneCollider : MonoBehaviour
{
    public string playerTag = "Player";
    public GameObject scaryScene;           // opcional: algo extra da cena master
    public AudioSource scarySound;          // som do susto master
    public SecondSequenceController secondSequenceController;

    bool hasTriggered = false;

    void Reset()
    {
        var col = GetComponent<Collider>();
        if (col) col.isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;
        if (!other.CompareTag(playerTag)) return;

        // Só deixa passar se o inimigo já foi mostrado na fase 2
        if (secondSequenceController == null || !secondSequenceController.EnemyRevealed)
        {
            // Ainda não está na hora; apenas sai (player pode voltar depois)
            // Debug.Log("Entrou cedo demais: inimigo ainda não foi revelado.");
            return;
        }

        hasTriggered = true;

        // Ativa objetos adicionais da cena master (se tiver)
        //if (scaryScene) scaryScene.SetActive(true);

        // // Toca o som do susto
        // if (scarySound && scarySound.clip)
        //     scarySound.PlayOneShot(scarySound.clip);

        StartCoroutine(ScaryCutSceneEndGame());
    }

    IEnumerator ScaryCutSceneEndGame()
    {
        if (scarySound && scarySound.clip)
            scarySound.PlayOneShot(scarySound.clip);

        // espera o som acabar
        if (scarySound && scarySound.clip)
            yield return new WaitForSeconds(scarySound.clip.length);

        // termina o jogo (você cuida do resto)
        Debug.Log(">> FIM DE JOGO POR CENA MASTER <<");
    }
}
