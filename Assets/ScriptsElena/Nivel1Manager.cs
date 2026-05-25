using UnityEngine;
using System.Collections;

public class Nivel1Manager : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DialogosInicio());
    }

    IEnumerator DialogosInicio()
    {
        yield return new WaitForSeconds(3f);
        DialogoManager.Instance.MostrarDialogo("No sé cómo llegué aquí...", 5f);
    }
}