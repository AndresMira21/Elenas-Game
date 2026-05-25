using UnityEngine;
using TMPro;
using System.Collections;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager Instance;
    public TextMeshProUGUI textoDialogo;
    public GameObject panelDialogo;
    public float tiempoEntreLetras = 0.05f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MostrarDialogo(string texto, float duracion = 5f)
    {
        StartCoroutine(MostrarTexto(texto, duracion));
    }

    IEnumerator MostrarTexto(string texto, float duracion)
    {
        panelDialogo.SetActive(true);
        textoDialogo.text = "";
        foreach (char letra in texto)
        {
            textoDialogo.text += letra;
            yield return new WaitForSeconds(tiempoEntreLetras);
        }
        yield return new WaitForSeconds(duracion);
        panelDialogo.SetActive(false);
    }
}