using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Opções de interação")]
    public string promptText = "Press E to interact"; // opcional: pode usar pra exibir na tela
    public UnityEvent onInteract; // define o que acontece quando interagir

    public void Interact()
    {
        onInteract?.Invoke();
    }
}
