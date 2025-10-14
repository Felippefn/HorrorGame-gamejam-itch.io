using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtObjects : MonoBehaviour
{

    public Text textObject;
    public string description = "Description";

    public bool inReach;


    void Start()
    {
        textObject.GetComponent<Text>().enabled = false;        
    }

    void OTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Reach")
        {
            textObject.GetComponent<Text>().enabled = true;
            //textObject.text = description;
            inReach = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Reach")
        {
            textObject.GetComponent<Text>().enabled = false;
            inReach = false;
            textObject.GetComponent<Text>().text = "";
        }
    }

    void Update()
    {
        if(inReach)
        {
            textObject.text = description.ToString();
        }
    }
}
