using UnityEngine;

public class DoorHandleController : MonoBehaviour
{
    public float openY = 90f;   // rotação da porta aberta
    public float closeY = 0f;   // rotação da porta fechada
    public float speed = 3f;    // velocidade de rotação
    public bool isOpen = false; // estado atual

    void Update()
    {
        // rotação atual
        float targetY = isOpen ? openY : closeY;
        Vector3 rot = transform.localEulerAngles;
        rot.y = Mathf.LerpAngle(rot.y, targetY, Time.deltaTime * speed);
        transform.localEulerAngles = rot;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
    }
}
