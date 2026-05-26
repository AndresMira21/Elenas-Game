using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BedSleep : MonoBehaviour
{
    public string mensajeNoDormir = "Aún no has explorado lo suficiente...";
    public string mensajeDormir = "Te acuestas a dormir...";
    public float tiempoMensaje = 2f;

    [Tooltip("Nombre exacto de la escena del ending en Build Settings")]
    public string escenaEnding = "Ending";

    [Tooltip("En qué día se va al ending (5 = después de dormir la noche 5)")]
    public int diaEnding = 5;

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

        GameManager.Instance.Sleep();

        yield return new WaitForSeconds(1f);

        // Si es la noche 5, ir al ending
        if (GameManager.Instance.currentDay > diaEnding)
        {
            SceneManager.LoadScene(escenaEnding);
            yield break;
        }

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