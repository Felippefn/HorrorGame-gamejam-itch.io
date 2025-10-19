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
            //print("player is in tv speak trigger");
            playerInTrigger = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //print("player left tv speak trigger");
            playerInTrigger = false;
        }
    }

}

