using UnityEditor;
using UnityEngine;

public class PlayerRaycastInteraction : MonoBehaviour
{
    public float interactDistance = 3f; // distância do raycast
    public LayerMask interactableLayer; // define uma layer "Interactable"
    public KeyCode interactKey = KeyCode.E;

    Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Lança um raio a partir do centro da câmera
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, interactDistance, interactableLayer))
        {
            // Se o objeto tem o componente Interactable
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                // Pode exibir UI de “Press E to Interact”
                if (Input.GetKeyDown(interactKey))
                {
                    interactable.Interact();
                }
            }
        }
    }
}
