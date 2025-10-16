using UnityEngine;

public class DoorHandleController : MonoBehaviour
{
    public float openY = 90f;   
    public float closeY = 0f;   
    public float speed = 3f;    
    public bool isOpen = false; 

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
