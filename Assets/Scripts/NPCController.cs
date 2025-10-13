using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCController : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform targetTV;     // ponto perto da TV
    public Transform exitPoint;    // corredor/porta de saída
    //public Animator anim;
    public float fixDuration = 3f;

    public System.Action OnFixed;  // callback

    void Awake()
    {
        if (!agent) agent = GetComponent<NavMeshAgent>();
    }

    // Move até um ponto
    public IEnumerator MoveTo(Vector3 dest)
    {
        agent.SetDestination(dest);
        while (agent.pathPending) yield return null;
        while (agent.remainingDistance > agent.stoppingDistance + 0.05f)
            yield return null;
    }

     // Move por um caminho (waypoints)
    public IEnumerator MoveThroughPath(WaypointPath path, float waitAtEach = 0f)
    {
        if (path == null || path.points.Count == 0) yield break;

        foreach (var p in path.points)
        {
            if (!p) continue;
            yield return MoveTo(p.position);
            if (waitAtEach > 0f) yield return new WaitForSeconds(waitAtEach);
        }

        if (path.loop && path.points.Count > 1)
        {
            // exemplo de loop simples
            while (true)
            {
                foreach (var p in path.points)
                {
                    if (!p) continue;
                    yield return MoveTo(p.position);
                    if (waitAtEach > 0f) yield return new WaitForSeconds(waitAtEach);
                }
            }
        }
    }

    public IEnumerator DoServiceWithPaths(WaypointPath toTV, WaypointPath toExit)
    {
        // Ir até a TV por waypoints (ou direto se não tiver)
        Debug.Log("Técnico indo até a TV...");
        if (toTV != null && toTV.points.Count > 0)
            yield return MoveThroughPath(toTV);
        else if (targetTV != null)
            yield return MoveTo(targetTV.position);

        // “Consertar”
        Debug.Log("Técnico consertando a TV...");
        yield return new WaitForSeconds(fixDuration);
        //OnFixed?.Invoke();

        // Sair por waypoints (ou direto)
        Debug.Log("Técnico saindo...");
        if (toExit != null && toExit.points.Count > 0)
            yield return MoveThroughPath(toExit);
        else if (exitPoint != null)
            yield return MoveTo(exitPoint.position);

        gameObject.SetActive(false);
    }
}
