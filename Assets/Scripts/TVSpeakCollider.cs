using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVSpeakCollider : MonoBehaviour
{
    public bool playerInTrigger = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

}

