using UnityEngine;

public class SitController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Player root object (standing state)")]
    public GameObject playerStanding;      // jogador em pé
    [Tooltip("Seat position & rotation target")]
    public Transform seatPoint;            // posição/rotação do assento

    [Header("State")]
    public bool sitting;
    public bool canStandUp = true;

    private PlayerMovemetController pmc;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Collider playerCollider;

    PlayerRaycastInteraction pRayIn;

    void Awake()
    {
        if (playerStanding == null)
        {
            Debug.LogError("[SitController] Missing playerStanding reference.");
            enabled = false;
            return;
        }

        pmc = playerStanding.GetComponent<PlayerMovemetController>();
        if (pmc == null)
            Debug.LogWarning("[SitController] PlayerMovemetController not found on playerStanding.");

        playerCollider = playerStanding.GetComponent<Collider>();
        if (playerCollider == null)
            Debug.LogWarning("[SitController] Player has no Collider component.");

        if (seatPoint == null)
        {
            Debug.LogError("[SitController] Missing seatPoint reference.");
            enabled = false;
            return;
        }

        pRayIn = playerStanding.GetComponent<PlayerRaycastInteraction>();
        sitting = false;
        canStandUp = true;
    }

    void Update()
    {
        // permitir levantar com WASD/Space, como você já fazia
        if (sitting && canStandUp && (
            Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.Space)))
        {
            StandUp();
        }
    }

    /// <summary>
    /// Chamado pelo UnityEvent do Interactable quando o jogador aperta E.
    /// Se não estiver sentado, senta. Se já estiver e puder levantar, levanta.
    /// </summary>
    public void OnInteract()
    {
        if (!sitting)
            SitDown();
        // else if (canStandUp)
        //     StandUp();
    }

    private void SitDown()
    {
        pRayIn.HidePrompt();           
        // guarda posição/rotação atuais
        originalPosition = playerStanding.transform.position;
        originalRotation = playerStanding.transform.rotation;

        // trava movimento + colisão
        if (pmc != null) pmc.canMove = false;
        if (playerCollider != null) playerCollider.enabled = false;

        // move para o assento
        playerStanding.transform.SetPositionAndRotation(seatPoint.position, seatPoint.rotation);

        sitting = true;
    }

    private void StandUp()
    {
        pRayIn.ShowPrompt(this.GetComponent<Interactable>());
        // volta para onde estava (ou use um empty exclusivo se preferir)
        playerStanding.transform.SetPositionAndRotation(originalPosition, originalRotation);

        if (playerCollider != null) playerCollider.enabled = true;
        if (pmc != null) pmc.canMove = true;

        sitting = false;
    }
}
