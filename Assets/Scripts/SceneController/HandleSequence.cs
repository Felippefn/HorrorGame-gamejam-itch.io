using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HandleSequence : MonoBehaviour
{
    private FirstSequenceController firstSequenceController;
    private SecondSequenceController secondSequenceController;
    private ThirdSequenceController thirdSequenceController;


    void Awake()
    {
        firstSequenceController = GetComponent<FirstSequenceController>();
        secondSequenceController = GetComponent<SecondSequenceController>();
        thirdSequenceController = GetComponent<ThirdSequenceController>();



        firstSequenceController.enabled = true;
        secondSequenceController.enabled = false;
        thirdSequenceController.enabled = false;    
    }

    // Update is called once per frame
    void Update()
    {
     if(firstSequenceController.StageSequenceDone)
        {
            firstSequenceController.enabled = false;
            secondSequenceController.enabled = true;
        }

        // if(secondSequenceController.StageSequenceDone)
        // {
        //     secondSequenceController.enabled = false;
        //     thirdSequenceController.enabled = true;
        // }
    }
}
