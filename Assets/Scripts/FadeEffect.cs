using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    public void FadeOutAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    IEnumerator FadeIn()
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            canvasGroup.alpha = 1 - (Time.time - startTime) / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 0;
    }

    IEnumerator FadeOut(string sceneName)
    {
        float startTime = Time.time;

        while (Time.time < startTime + fadeDuration)
        {
            canvasGroup.alpha = (Time.time - startTime) / fadeDuration;
            yield return null;
        }

        canvasGroup.alpha = 1;

        SceneManager.LoadScene(sceneName);
    }
}
