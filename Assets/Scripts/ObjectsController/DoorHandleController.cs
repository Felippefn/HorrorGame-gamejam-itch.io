using UnityEngine;

public class DoorHandleController : MonoBehaviour
{
    public float openY = 90f;   
    public float closeY = 0f;   
    public float speed = 3f;
    public bool isOpen = false; 
    public bool canBeInteracted = true;

    void Update()
    {
    
            float targetY = isOpen ? openY : closeY;
            Vector3 rot = transform.localEulerAngles;
            rot.y = Mathf.LerpAngle(rot.y, targetY, Time.deltaTime * speed);
            transform.localEulerAngles = rot;
    }

    public void ToggleDoor()
    {
        if (canBeInteracted)
        {
            isOpen = !isOpen;
        }
        else
        {
            print("I shouldn't be able to open the door now.");
        }
    }
}
