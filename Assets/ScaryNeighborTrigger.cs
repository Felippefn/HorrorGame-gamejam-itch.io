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

    int hasRun = 0;                 // 0 -> primeira vez; 1 -> segunda; etc.
    DoorHandleController dhc;
    bool wasOpen = false;           // guarda estado anterior da porta p/ detectar "acabou de abrir"

    void Awake()
    {
        if (door)
            dhc = door.GetComponent<DoorHandleController>();
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (dhc == null) {
            Debug.LogWarning("[ScaryNeighborTrigger] DoorHandleController não encontrado.");
            return;
        }

        bool nowOpen = dhc.isOpen;

        // dispara apenas quando a porta acabou de ficar ABERTA (transição fechada -> aberta)
        if (nowOpen && !wasOpen)
        {
            switch (hasRun)
            {
                case 0:
                    Debug.Log("[ScaryNeighborTrigger] 🎬 Evento 0: primeira abertura.");
                    if (neighboor) neighboor.SetActive(true);
                    onScary?.Invoke();
                    break;

                case 1:
                    Debug.Log("[ScaryNeighborTrigger] 🎬 Evento 1: segunda abertura.");
                    // TODO: coloque aqui o segundo evento (outro UnityEvent ou chamada)
                    onScary?.Invoke();
                    break;

                case 2:
                    Debug.Log("[ScaryNeighborTrigger] 🎬 Evento 2: terceira abertura.");
                    // TODO: terceiro evento
                    onScary?.Invoke();
                    break;

                default:
                    Debug.Log("[ScaryNeighborTrigger] Já executou todos os eventos.");
                    break;
            }

            hasRun++; // 👉 incrementa SÓ quando abriu agora
        }

        // atualiza o estado anterior
        wasOpen = nowOpen;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        var c = GetComponent<BoxCollider>();
        if (c)
            Gizmos.DrawWireCube(c.bounds.center, c.bounds.size);
    }
}
