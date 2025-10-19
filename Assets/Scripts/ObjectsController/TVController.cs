using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;

public class TVController : MonoBehaviour
{
    // public Animator anim;
    // public AudioSource voice;
    // public AudioClip[] lines;

    [Header("TV UI Indicators")]
    //public TextMeshProUGUI powerIndicatorUI;
    //public TextMeshProUGUI brokenIndicatorUI;
    

    public float delayBetweenLines = 1.2f;

    
    public bool IsBroken { get; private set; } = true; 


    public bool IsOn { get; private set; }

    public bool _SpokeWarningLine { get; private set;} = false;


    // void Awake()
    // {
    //     powerIndicatorUI.gameObject.SetActive(false);
    // }

    public void SetBroken(bool broken)
    {
        IsBroken = broken;
        if (IsBroken && IsOn)
        {
            IsOn = false;
            //StartCoroutine(ShowBrokenMessageIndicator());
            // if (anim) anim.SetBool("On", false);
        }
    }

    public void SetPower(bool on)
    {
        if (on && IsBroken)
        {
            //StartCoroutine(ShowBrokenMessageIndicator());
            IsOn = false;
            // if (anim) anim.SetBool("On", false);
            return;
        }

        IsOn = on;
        print("TV " + (on ? "ligada" : "desligada"));
        // if (anim) anim.SetBool("On", on);
    }

    public void SetSpokenWarningLine(bool spoke)
    {
        _SpokeWarningLine = spoke;
    }

    public void SpeakSequence()
    {
        if (IsBroken)
        {
            //StartCoroutine(ShowPowerMessageIndicator());
            return;
        }
        if (!IsOn)
        {
            //StartCoroutine(ShowPowerMessageIndicator());
            return;
        }

        StartCoroutine(PlayWarningLine());
    }
    public void InteractionTV()
    {
        if (IsBroken)
        {
            //StartCoroutine(ShowPowerMessageIndicator());
            return;
        }
        if (!IsOn)
        {
            //StartCoroutine(ShowPowerMessageIndicator());
            return;
        }
        if (_SpokeWarningLine)
        {
            print("TV já falou a linha de aviso, não troca de canal");
            return;
        }
        
        print("Trocando de canal na TV");
    }

    IEnumerator PlayWarningLine()
    {
        SetSpokenWarningLine(true);
        print("WAIT");
        yield return new WaitForSeconds(1f);
        print("DO NOT OPEN THAT DOOR");
    }

    // IEnumerator ShowPowerMessageIndicator()
    // {
    //     powerIndicatorUI.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(2f);
    //     powerIndicatorUI.gameObject.SetActive(false);
    // }
    // IEnumerator ShowBrokenMessageIndicator()
    // {
    //     brokenIndicatorUI.gameObject.SetActive(true);
    //     yield return new WaitForSeconds(2f);
    //     brokenIndicatorUI.gameObject.SetActive(false);
    // }

}
