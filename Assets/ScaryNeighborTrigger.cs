using UnityEngine;
using UnityEngine.Events;

public class ScaryNeighborTrigger : MonoBehaviour
{
    [Header("Refs")]
    public GameObject door;
    public GameObject neighboor;        
    public UnityEvent onScary;      

    [Header("Config")]
    public string playerTag = "Player";

    bool hasRun = false;
    DoorHandleController dhc;

    void Awake()
    {
        if (door)
            dhc = door.GetComponent<DoorHandleController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (hasRun) return;
        if (!other.CompareTag(playerTag)) return;
        if (door == null)
        {
            Debug.LogWarning("[ScaryNeighborTrigger] DoorHandleController não setado.");
            return;
        }

        if (IsDoorConsideredOpen())
        {
            hasRun = true;
            Debug.Log("[ScaryNeighborTrigger] 🎬 Evento: player no trigger e porta aberta.");

            if (neighboor) neighboor.SetActive(true); // se quiser
            onScary?.Invoke();                         // chama eventos do Inspector

            // Coloque aqui o que quiser fazer:
            // StartCoroutine(SequenciaAssustadora());
        }
    }

    void OnDrawGizmos()
{
    Gizmos.color = Color.red;
    var c = GetComponent<BoxCollider>();
    if (c)
        Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
}


    bool IsDoorConsideredOpen()
    {
        return dhc != null && dhc.isOpen; // muda se quiser usar outro critério
    }
}
