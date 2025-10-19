using UnityEngine;
using UnityEngine.Events;

public class ConfirmationUI : MonoBehaviour
{
    [Header("Ações a executar")]
    public UnityEvent onConfirm;
    public UnityEvent onCancel;

    [Header("Referências")]
    public GameObject panel;

    bool showing = false;

    void Start()
    {
        if (panel) panel.SetActive(false);
    }

    public void Ask()
    {
        if (showing) return;
        showing = true;
        if (panel) panel.SetActive(true);

        // pausa o jogo e libera o cursor
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Confirm()
    {
        print("Confirmado");
        Close();
        onConfirm?.Invoke();
    }

    public void Cancel()
    {
        print("Cancelado");
        Close();
        onCancel?.Invoke();
    }

    void Close()
    {
        print("deveria closar");
        if (panel) panel.SetActive(false);
        showing = false;

        // volta ao normal
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
