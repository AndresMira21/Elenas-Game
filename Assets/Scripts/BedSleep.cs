using UnityEngine;
using System.Collections;

public class BedSleep : MonoBehaviour
{
    public string mensajeNoDormir = "Aún no has explorado lo suficiente...";
    public string mensajeDormir = "Te acuestas a dormir...";

    public float tiempoMensaje = 2f;

    public void Interact()
    {
        // 🚫 NO PUEDE DORMIR
        if (!GameManager.Instance.CanSleep())
        {
            StartCoroutine(ShowMessage(mensajeNoDormir));
            return;
        }

        // ✅ PUEDE DORMIR
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        yield return ShowMessage(mensajeDormir);

        yield return ScreenFade.Instance.FadeToBlack(2f);

        GameManager.Instance.Sleep();

        yield return new WaitForSeconds(1f);

        yield return ScreenFade.Instance.FadeFromBlack(2f);

        PlayerMovement.canMove = true;
        PlayerMovement.canLook = true;
    }

    IEnumerator ShowMessage(string msg)
    {
        DialogueManager.Instance.ShowThought(msg, tiempoMensaje);
        yield return new WaitForSeconds(tiempoMensaje);
    }
}