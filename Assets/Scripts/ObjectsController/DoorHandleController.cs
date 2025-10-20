using System.Collections;
using TMPro;
using UnityEngine;

public class DoorHandleController : MonoBehaviour
{
    [Header("Rotação")]
    public float openY = 90f;
    public float closeY = 0f;
    public float speed = 3f;

    [Header("Estado")]
    public bool isOpen = false;
    public bool canBeInteracted = true;
    public bool playerInteracted = false;

    [Header("UI")]
    public TextMeshProUGUI doNotTextMesh;

    [Header("Auto-close")]
    public bool autoClose = true;
    public float autoCloseDelay = 5f;

    // controle interno
    int _openTicket = 0;     // identifica a "sessão" atual de abertura

    void Update()
    {
        float targetY = isOpen ? openY : closeY;
        Vector3 rot = transform.localEulerAngles;
        rot.y = Mathf.LerpAngle(rot.y, targetY, Time.deltaTime * speed);
        transform.localEulerAngles = rot;
    }

    public void ToggleDoor()
    {
        Debug.Log("Tentou interagir com a porta");
        playerInteracted = true;

        if (canBeInteracted)
        {
            // alterna estado
            bool willOpen = !isOpen;
            isOpen = willOpen;

            if (willOpen && autoClose)
            {
                // inicia um novo "ticket" de abertura
                _openTicket++;
                StartCoroutine(AutoCloseAfterDelay(_openTicket));
            }
            else
            {
                // se fechou manualmente, invalida tickets antigos
                _openTicket++;
            }
        }
        else
        {
            StartCoroutine(ShowDoNotText());
        }

        // reseta a flag no próximo frame (para outros scripts capturarem a borda)
        StartCoroutine(ClearInteractedFlagNextFrame());
    }

    IEnumerator AutoCloseAfterDelay(int ticket)
    {
        yield return new WaitForSeconds(autoCloseDelay);

        // Só fecha se ainda estiver aberta E se este ticket ainda for o vigente
        if (isOpen && ticket == _openTicket)
        {
            isOpen = false;
        }
    }

    IEnumerator ClearInteractedFlagNextFrame()
    {
        yield return null; // espera 1 frame
        playerInteracted = false;
    }

    IEnumerator ShowDoNotText()
    {
        if (doNotTextMesh != null)
        {
            doNotTextMesh.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            doNotTextMesh.gameObject.SetActive(false);
        }
    }

    // Opcional: fechar à força via outros scripts
    public void ForceClose()
    {
        _openTicket++; // invalida timers antigos
        isOpen = false;
    }
}
