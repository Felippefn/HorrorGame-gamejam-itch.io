using UnityEngine;
using UnityEngine.UI;

public class PlayerRaycastInteraction : MonoBehaviour
{
    [Header("Interação")]
    public float interactDistance = 1f; // distância do raycast
    public LayerMask interactableLayer; // layer dos objetos interagíveis
    public KeyCode interactKey = KeyCode.E;

    [Header("UI")]
    public Text interactText; // arraste o texto do Canvas aqui
    public string defaultPrompt = "Press E to interact";

    private Camera cam;
    private Interactable currentInteractable;

    void Start()
    {
        cam = Camera.main;
        if (interactText != null)
            interactText.gameObject.SetActive(false);
    }

    void Update()
    {
        HandleInteractionRaycast();
    }

    void HandleInteractionRaycast()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Mostra o texto de interação
                ShowPrompt(interactable);

                // Interagir
                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact();
                }

                currentInteractable = interactable;
                return;
            }
        }

        // Se não estiver olhando pra nada interagível → some o texto
        HidePrompt();
    }

    public void ShowPrompt(Interactable interactable)
    {
        if (interactText == null) return;

        string displayText = string.IsNullOrEmpty(interactable.customPrompt)
            ? defaultPrompt
            : interactable.customPrompt;

        interactText.text = displayText;
        interactText.gameObject.SetActive(true);
    }

    public void HidePrompt()
    {
        if (interactText != null)
            interactText.gameObject.SetActive(false);

        currentInteractable = null;
    }
}
