using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    public string customPrompt = "Press E to interact";
    //public string customText = "You interacted with something!";

    public UnityEvent onInteract;

    public void Interact()
    {
        onInteract?.Invoke();
    }

    // public void DefaultMessageUnableToInteract(string text)
    // {
    //     customText = text;
    // }
}
