using UnityEngine;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using Microsoft.Unity.VisualStudio.Editor;

public class TVController : MonoBehaviour
{
    // public Animator anim;
    // public AudioSource voice;
    // public AudioClip[] lines;

    [Header("TV UI Indicators")]
    public TextMeshProUGUI powerIndicatorUI;
    public TextMeshProUGUI brokenIndicatorUI;
    

    public float delayBetweenLines = 1.2f;

    
    public bool IsBroken { get; private set; } = true; 


    public bool IsOn { get; private set; }

    public bool _SpokeWarningLine { get; private set; } = false;

    public FirstSequenceController firstSequenceController;

    [Header("Dialog References / UI")]
    public TextMeshProUGUI WaitDialog;
    public AudioSource WaitAudioSource;
    public GameObject TVContentImage;

    public TextMeshProUGUI DoNotOpenDialog;
    //public AudioSource DoNotAudioSource;

    public TextMeshProUGUI TimeToExplainDialog;
    public AudioSource TimeToExplainAudioSource;


    public TextMeshProUGUI CheckBathroomDialog;
    public AudioSource CheckBathroomAudioSource;


    void Awake()
    {
        powerIndicatorUI.gameObject.SetActive(false);
        brokenIndicatorUI.gameObject.SetActive(false);
        TVContentImage.gameObject.SetActive(false);


        if (WaitDialog)
            WaitDialog.gameObject.SetActive(false);
        if (DoNotOpenDialog)
            DoNotOpenDialog.gameObject.SetActive(false);
        if (TimeToExplainDialog)
            TimeToExplainDialog.gameObject.SetActive(false);
        if (CheckBathroomDialog)
            CheckBathroomDialog.gameObject.SetActive(false);
    }

    public void WrapperStartDialog(TextMeshProUGUI textMesh)
    {
        StartCoroutine(StartDialog(textMesh));
    }

    IEnumerator StartDialog(TextMeshProUGUI textMesh)
    {
        textMesh.gameObject.SetActive(true);
        textMesh.text = textMesh.text.Trim(); // garante texto limpo

        string fullText = textMesh.text;
        textMesh.text = "";

        foreach (char c in fullText)
        {
            textMesh.text += c;
            yield return new WaitForSeconds(0.05f); // velocidade da digitação
        }

        yield return new WaitForSeconds(2f); // tempo de exibição
        textMesh.gameObject.SetActive(false);
    }


    public void SetBroken(bool broken)
    {
        IsBroken = broken;
        if (IsBroken && IsOn)
        {
            IsOn = false;
            StartCoroutine(ShowBrokenMessageIndicator());
            // if (anim) anim.SetBool("On", false);
        }
    }

    public void SetPower(bool on)
    {
        if (on && IsBroken)
        {
            StartCoroutine(ShowBrokenMessageIndicator());
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
        StartCoroutine(PlayWarningLine());
    }
    public void InteractionTV()
    {
        if (IsBroken)
        {
            StartCoroutine(ShowBrokenMessageIndicator());
            return;
        }
        if (!IsOn)
        {
            StartCoroutine(ShowPowerMessageIndicator());
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
        WaitAudioSource.PlayOneShot(WaitAudioSource.clip);
        StartCoroutine(StartDialog(WaitDialog));
        yield return new WaitForSeconds(1f);
        StartCoroutine(StartDialog(DoNotOpenDialog));
        yield return new WaitForSeconds(3.5f);
        firstSequenceController.WrapperStartDialog(firstSequenceController.playerQuestioningTV);
        yield return new WaitForSeconds(3f);
        TimeToExplainAudioSource.PlayOneShot(TimeToExplainAudioSource.clip);
        StartCoroutine(StartDialog(TimeToExplainDialog));


        SetSpokenWarningLine(true);
    }

    IEnumerator ShowPowerMessageIndicator()
    {
        powerIndicatorUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        powerIndicatorUI.gameObject.SetActive(false);
    }
    IEnumerator ShowBrokenMessageIndicator()
    {
        brokenIndicatorUI.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        brokenIndicatorUI.gameObject.SetActive(false);
    }

}
