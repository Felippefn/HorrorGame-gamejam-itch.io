using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class EndScreen : MonoBehaviour
{
    [Header("UI References")]
    public Image blackImage;                 // Image preta full screen
    public TextMeshProUGUI continueText;     // “CONTINUA…”
    public AudioSource laughTV;              // som opcional de fundo

    [Header("Settings")]
    public float fadeTime = 1.5f;
    public bool waitForAnyKey = true;



    void Awake()
    {
        if (blackImage)
        {
            Color c = blackImage.color;
            c.a = 0f;
            blackImage.color = c;
        }

        if (continueText)
            continueText.gameObject.SetActive(false);

        blackImage.gameObject.SetActive(false);
        continueText.gameObject.SetActive(false);
    }

    public void PlayEnd()
    {
        blackImage.gameObject.SetActive(true);
        StartCoroutine(FadeEnd());
    }

    IEnumerator FadeEnd()
    {
        float startVol = AudioListener.volume;
        float t = 0f;

        // toca risada da TV se tiver
        if (laughTV && laughTV.clip)
            laughTV.Play();

        // Fade in da imagem preta
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            float a = Mathf.Clamp01(t / fadeTime);

            if (blackImage)
            {
                Color c = blackImage.color;
                c.a = a;
                blackImage.color = c;
            }

            AudioListener.volume = Mathf.Lerp(startVol, 0f, a);
            yield return null;
        }

        // garante total preto
        if (blackImage)
        {
            Color c = blackImage.color;
            c.a = 1f;
            blackImage.color = c;
        }
        AudioListener.volume = 0f;

        // mostra o texto "CONTINUA..."
        if (continueText) continueText.gameObject.SetActive(true);

        // espera o input do jogador
        if (waitForAnyKey)
        {
            while (!Input.anyKeyDown)
                yield return null;
        }

        Next();
    }

    public void Next()
    {
        print("Encerrando jogo...");
        Application.Quit();
    //     // o que fazer depois do fade
    //     if (SceneManager.sceneCountInBuildSettings > 1)
    //         SceneManager.LoadScene(0); // 0 = menu
    //     else
    //         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
     }
}
