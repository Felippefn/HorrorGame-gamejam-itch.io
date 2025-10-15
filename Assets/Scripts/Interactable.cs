using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    public string customPrompt = "Press E to interact";

    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract?.Invoke();
    }
}
