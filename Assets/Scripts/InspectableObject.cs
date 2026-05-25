using UnityEngine;
using System.Collections;

public class InspectableObject : MonoBehaviour
{
    [TextArea]
    public string message;

    public float zoomFOV = 40f;
    public float zoomSpeed = 5f;

    Camera cam;
    float normalFOV;

    void Start()
    {
        cam = Camera.main;
        normalFOV = cam.fieldOfView;
    }

    public void Inspect()
    {
        GameManager.Instance.RegisterExploration();
        
        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        StartCoroutine(InspectRoutine());
    }

    IEnumerator InspectRoutine()
    {
        // 🔍 zoom in suave
        yield return StartCoroutine(ChangeFOV(zoomFOV));

        // mostrar texto
        DialogueManager.Instance.ShowThought(message, 3f);

        yield return new WaitForSeconds(3f);

        // 🔍 zoom out suave
        yield return StartCoroutine(ChangeFOV(normalFOV));

        // restaurar control
        PlayerMovement.canMove = true;
        PlayerMovement.canLook = true;
    }

    IEnumerator ChangeFOV(float target)
    {
        while (Mathf.Abs(cam.fieldOfView - target) > 0.1f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, target, Time.deltaTime * zoomSpeed);
            yield return null;
        }

        cam.fieldOfView = target;
    }
}