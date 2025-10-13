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

    public IEnumerator DoService()
    {
        // Chega até a TV
        print("NPC GOING TO TV");
        agent.SetDestination(targetTV.position);
        while (Vector3.Distance(transform.position, targetTV.position) > agent.stoppingDistance + 1f)
             yield return null;


        // “Consertar”
        print("NPC FIXING TV");
        //if (anim) anim.SetTrigger("Fix");
        yield return new WaitForSeconds(fixDuration);
        //OnFixed?.Invoke();

        // Vai embora
        print("NPC EXITING");
        agent.SetDestination(exitPoint.position);
        while (Vector3.Distance(transform.position, exitPoint.position) > agent.stoppingDistance + 0.1f)
            yield return null;

        // some/disable
        gameObject.SetActive(false);
    }
}
