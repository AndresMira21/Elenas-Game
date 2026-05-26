using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    public PlayerMovement playerMovement;

    [Header("Altura")]
    public float startHeight = 0.5f;
    public float normalHeight = 1.9f;

    [Header("Velocidad al levantarse")]
    public float riseSpeed = 1f;

    IEnumerator Start()
    {
        // bloquear control
        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        // empezar acostado
        playerMovement.SetCameraHeight(startHeight);

        // pequeño silencio
        yield return new WaitForSeconds(1f);

        // fade
        yield return ScreenFade.Instance.FadeFromBlack(2f);

        // pensamiento inicial
        DialogueManager.Instance.ShowThought(
            "¿Qué hora es...?",
            3f
        );

        yield return new WaitForSeconds(4f);

        // tutorial cámara
        DialogueManager.Instance.ShowThought(
            "Mueve el mouse para mirar alrededor.",
            4f
        );

        // permitir cámara
        PlayerMovement.canLook = true;

        yield return new WaitForSeconds(4.5f);

        // levantarse lentamente
        float current = startHeight;

        while (current < normalHeight)
        {
            current += Time.deltaTime * riseSpeed;

            playerMovement.SetCameraHeight(current);

            yield return null;
        }

        playerMovement.SetCameraHeight(normalHeight);

        // guardar altura correcta
        playerMovement.camaraOriginal =
            playerMovement.playerCamera.localPosition;

        Debug.Log(
            "camaraOriginal al final de intro: "
            + playerMovement.camaraOriginal.y
        );

        yield return new WaitForSeconds(1f);

        // tutorial movimiento
        DialogueManager.Instance.ShowThought(
            "Usa WASD para moverte.",
            4f
        );

        // permitir movimiento
        PlayerMovement.canMove = true;

        yield return new WaitForSeconds(3f);

        // tutorial interacción
        DialogueManager.Instance.ShowThought(
            "Presiona E para interactuar.",
            4f
        );

        yield return new WaitForSeconds(2f);
        // tutorial agacharse
        DialogueManager.Instance.ShowThought(
            "Presiona C para agacharte.",
            4f
        );
    }
}