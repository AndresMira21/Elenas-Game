using UnityEngine;
using System.Collections;

public class BedSleep : MonoBehaviour
{
    public string mensajeNoDormir = "Aún no has explorado lo suficiente...";
    public string mensajeDormir = "Te acuestas a dormir...";
    public float tiempoMensaje = 2f;

    public void Interact()
    {
        if (!GameManager.Instance.CanSleep())
        {
            StartCoroutine(ShowMessage(mensajeNoDormir));
            return;
        }
        StartCoroutine(SleepRoutine());
    }

    IEnumerator SleepRoutine()
    {
        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        yield return ShowMessage(mensajeDormir);
        yield return ScreenFade.Instance.FadeToBlack(2f);

        // Sleep() sube el día — DemoTrigger lo detecta en el mismo frame
        GameManager.Instance.Sleep();

        yield return new WaitForSeconds(1f);

        // Si la demo terminó, no hacemos fade de vuelta ni reactivamos movimiento
        // El Canvas del final ya está visible encima
        if (DemoTrigger.DemoTerminada)
            yield break;

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