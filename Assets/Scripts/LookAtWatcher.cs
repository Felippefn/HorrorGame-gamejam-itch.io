using UnityEngine;
using UnityEngine.Events;

public class LookAtWatcher : MonoBehaviour
{
    public Camera cam;                 // Main Camera
    public Transform target;           // o ponto/vaso/centro do espelho
    public float maxAngle = 15f;       // tolerância de olhar
    public float maxDistance = 2f;     // alcance
    public bool once = true;
    public UnityEvent onLook;
    public FirstSequenceController firstSequenceController;

    bool fired;

    void Update()
    {
        if (once && fired) return;
        if (!cam || !target) return;

        Vector3 to = target.position - cam.transform.position;
        float dist = to.magnitude;
        if (dist > maxDistance) return;

        float angle = Vector3.Angle(cam.transform.forward, to.normalized);
        if (angle <= maxAngle && firstSequenceController.StageSequenceDone)
        {
            fired = true;
            onLook?.Invoke();
        }
    }

    public void ResetWatch() => fired = false;
}
