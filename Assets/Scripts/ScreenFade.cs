using UnityEngine;
using System.Collections;

public class ScreenFade : MonoBehaviour
{
    public static ScreenFade Instance;

    public CanvasGroup fadePanel;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        fadePanel.alpha = 0;
        fadePanel.gameObject.SetActive(false);
    }

    public IEnumerator FadeToBlack(float speed = 2f)
    {
        if (fadePanel == null)
        {
            Debug.LogError("FadePanel no asignado");
            yield break;
        }

        fadePanel.gameObject.SetActive(true);

        while (fadePanel.alpha < 1)
        {
            fadePanel.alpha += Time.deltaTime * speed;
            yield return null;
        }
    }

    public IEnumerator FadeFromBlack(float speed = 2f)
    {
        while (fadePanel.alpha > 0)
        {
            fadePanel.alpha -= Time.deltaTime * speed;
            yield return null;
        }

        fadePanel.gameObject.SetActive(false);
    }
}