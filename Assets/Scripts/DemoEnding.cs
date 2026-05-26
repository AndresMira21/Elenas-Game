using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// DemoEnding.cs — Elena's Game
/// Activa el canvas final y lanza la secuencia de créditos animados.
/// </summary>
public class DemoEnding : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvasFinalDemo;

    [Header("Créditos animados")]
    public DemoCreditsUI creditsUI;

    void Start()
    {
        if (canvasFinalDemo != null)
            canvasFinalDemo.SetActive(false);
    }

    public void MostrarFinal()
    {
        if (canvasFinalDemo != null)
            canvasFinalDemo.SetActive(true);

        StartCoroutine(MostrarConFade());

        var gm = GameManager.Instance;
        Debug.Log("══════════════════════════════════\n" +
                  $"Negación: {gm.negacion} | Duda: {gm.duda} | Aceptación: {gm.aceptacion}\n" +
                  "══════════════════════════════════");
    }

    IEnumerator MostrarConFade()
    {
        Debug.Log("⏳ Esperando 1 segundo...");
        yield return new WaitForSeconds(1f);

        Debug.Log("🌅 Iniciando FadeFromBlack...");
        yield return ScreenFade.Instance.FadeFromBlack(2f);

        Debug.Log("🎬 Iniciando créditos...");
        if (creditsUI == null)
            Debug.LogError("❌ CreditsUI es null — no está asignado en el Inspector");
        else
            creditsUI.IniciarCreditos();
    }
}