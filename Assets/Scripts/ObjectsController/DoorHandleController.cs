using System.Collections;
using TMPro;
using UnityEngine;

public class DoorHandleController : MonoBehaviour
{
    public float openY = 90f;   
    public float closeY = 0f;   
    public float speed = 3f;
    public bool isOpen = false;
    public bool canBeInteracted = true;
    public bool playerInteracted = false;
    public TextMeshProUGUI doNotTextMesh;

    void Update()
    {
    
            float targetY = isOpen ? openY : closeY;
            Vector3 rot = transform.localEulerAngles;
            rot.y = Mathf.LerpAngle(rot.y, targetY, Time.deltaTime * speed);
            transform.localEulerAngles = rot;
    }

    public void ToggleDoor()
    {
        print("Tentou interagir com a porta");
        playerInteracted = true;
        print("player interacted : " + playerInteracted);
        if (canBeInteracted)
        {
            isOpen = !isOpen;
        }
        else
        {
            StartCoroutine(ShowDoNotText());
        }
        playerInteracted = false;
        print("player interacted : " + playerInteracted);
    }

    IEnumerator ShowDoNotText()
    {
        if (doNotTextMesh != null)
        {
            doNotTextMesh.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f);
            doNotTextMesh.gameObject.SetActive(false);
        }
    }
}
