using UnityEngine;
using System.Collections;

public class BedSleep : MonoBehaviour
{
    public string mensaje = "Te acuestas a dormir...";

    public void Interact()
    {
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        DialogueManager.Instance.ShowThought(mensaje, 2f);

        yield return new WaitForSeconds(2f);

        yield return ScreenFade.Instance.FadeToBlack(2f);

        GameManager.Instance.Sleep();

        yield return new WaitForSeconds(0.5f);

        yield return ScreenFade.Instance.FadeFromBlack(2f);
    }
}