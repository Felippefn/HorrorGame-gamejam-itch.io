using UnityEngine;

public class SitController : MonoBehaviour
{
    [Header("Referências")]
    public GameObject playerStanding;   // jogador em pé
    public GameObject intText;          // "Pressione E"
    //public GameObject standText;        // "Levantar (WASD/Espaço)"
    public Transform seatPoint;         // posição e rotação do assento

    [Header("State")]
    public bool interactable;
    public bool sitting;
    public bool canStandUp ;

    private PlayerMovemetController pmc;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    void Awake()
    {
        pmc = playerStanding.GetComponent<PlayerMovemetController>();
        intText.SetActive(false);
        //standText.SetActive(false);
        sitting = false;
        canStandUp = true;
        interactable = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactable = true;
            if (!sitting) intText.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactable = false;
            intText.SetActive(false);
        }
    }

    void Update()
    {
        if (interactable && !sitting && Input.GetKeyDown(KeyCode.E))
        {
            SitDown();
        }

        if (canStandUp && sitting && (
            Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) ||
            Input.GetKeyDown(KeyCode.Space)))
        {
            StandUp();
        }
    }

    void SitDown()
    {
        // guarda posição original
        originalPosition = playerStanding.transform.position;
        originalRotation = playerStanding.transform.rotation;

        // trava movimento
        pmc.canMove = false;
        playerStanding.GetComponent<Collider>().enabled = false;
        // move para o ponto do sofá
        playerStanding.transform.SetPositionAndRotation(seatPoint.position, seatPoint.rotation);

        // UI
        intText.SetActive(false);
        //standText.SetActive(true);

        sitting = true;
        interactable = false;
    }

    void StandUp()
    {
        // volta para a posição anterior (ou poderia colocar um empty "ponto de pé")
        playerStanding.transform.SetPositionAndRotation(originalPosition, originalRotation);
        playerStanding.GetComponent<Collider>().enabled = true;
        // libera movimento
        pmc.canMove = true;

        // UI
        //standText.SetActive(false);
        if (interactable) intText.SetActive(true);

        sitting = false;
    }
}
