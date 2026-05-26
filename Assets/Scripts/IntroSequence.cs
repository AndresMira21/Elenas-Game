using UnityEngine;
using System.Collections;

public class IntroSequence : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public float startHeight = 0.5f;
    public float normalHeight = 1.9f;
    public float riseSpeed = 1f;

    IEnumerator Start()
    {
        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        // empezar abajo
        playerMovement.SetCameraHeight(startHeight);
        yield return new WaitForSeconds(1f);
        yield return ScreenFade.Instance.FadeFromBlack(1f);

        DialogueManager.Instance.ShowThought(
            "¿Qué hora es...?",
            3f
        );

        yield return new WaitForSeconds(2f);

        // subir lentamente
        float current = startHeight;
        while (current < normalHeight)
        {
            current += Time.deltaTime * riseSpeed;
            playerMovement.SetCameraHeight(current);
            yield return null;
        }

        playerMovement.SetCameraHeight(normalHeight);
        playerMovement.camaraOriginal = playerMovement.playerCamera.localPosition;
        Debug.Log("camaraOriginal al final de intro: " + playerMovement.camaraOriginal.y);
        PlayerMovement.canMove = true;
        PlayerMovement.canLook = true;
    }
}