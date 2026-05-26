using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

/// <summary>
/// DemoCreditsUI.cs — Elena's Game
/// Secuencia animada de créditos para el final de la demo.
/// </summary>
public class DemoCreditsUI : MonoBehaviour
{
    [Header("Panel de fondo")]
    public Image panelFondo;

    [Header("Textos")]
    public TextMeshProUGUI tituloText;
    public TextMeshProUGUI cuerpoText;
    public TextMeshProUGUI citaText;
    public TextMeshProUGUI estadoText;
    public TextMeshProUGUI demoTagText;

    [Header("Tiempos")]
    public float velocidadFade = 1f;
    public float tiempoLectura = 4f;
    public float pausaEntreTextos = 0.8f;

    void Awake()
    {
        // Forzar alpha 0 en todos desde el inicio
        EsconderTexto(tituloText);
        EsconderTexto(cuerpoText);
        EsconderTexto(citaText);
        EsconderTexto(estadoText);
        EsconderTexto(demoTagText);
    }

    void OcultarTodo()
    {
        EsconderTexto(tituloText);
        EsconderTexto(cuerpoText);
        EsconderTexto(citaText);
        EsconderTexto(estadoText);
        EsconderTexto(demoTagText);
    }

    void EsconderTexto(TextMeshProUGUI txt)
    {
        if (txt == null) return;
        txt.alpha = 0f;
    }

    public void IniciarCreditos()
    {
        var gm = GameManager.Instance;
        string est = GetEstado(gm);

        // Asignar textos
        if (tituloText != null) tituloText.text = GetTitulo(est);
        if (cuerpoText != null) cuerpoText.text = GetCuerpo(est);
        if (citaText != null) citaText.text = GetCita(est);
        if (estadoText != null) estadoText.text = GetEstadoHUD(gm, est);
        if (demoTagText != null) demoTagText.text = "Elena's Game — Demo\nLa historia continúa.";

        // Forzar color blanco visible en todos
        SetColorBlanco(tituloText);
        SetColorBlanco(cuerpoText);
        SetColorBlanco(citaText);
        SetColorBlanco(estadoText);
        SetColorBlanco(demoTagText);

        StartCoroutine(SecuenciaCreditos());
    }

    void SetColorBlanco(TextMeshProUGUI txt)
    {
        if (txt == null) return;
        txt.color = new Color(1f, 1f, 1f, 0f); // blanco, alpha 0 (el fade lo sube)
    }

    IEnumerator SecuenciaCreditos()
    {
        // 1 — Título
        yield return Mostrar(tituloText);
        yield return new WaitForSeconds(tiempoLectura);
        yield return Ocultar(tituloText, velocidadFade);
        yield return new WaitForSeconds(pausaEntreTextos);

        // 2 — Cuerpo
        yield return Mostrar(cuerpoText);
        yield return new WaitForSeconds(tiempoLectura + 2f);
        yield return Ocultar(cuerpoText, velocidadFade);
        yield return new WaitForSeconds(pausaEntreTextos);

        // 3 — Cita de Elena
        yield return Mostrar(citaText);
        yield return new WaitForSeconds(tiempoLectura + 1f);
        yield return Ocultar(citaText, velocidadFade);
        yield return new WaitForSeconds(pausaEntreTextos);

        // 4 — Estado
        yield return Mostrar(estadoText);
        yield return new WaitForSeconds(tiempoLectura);
        yield return Ocultar(estadoText, velocidadFade);
        yield return new WaitForSeconds(pausaEntreTextos);

        // 5 — Tag final (se queda)
        yield return Mostrar(demoTagText);
    }

    // ─── Fade in ──────────────────────────────────────────────────────────────
    IEnumerator Mostrar(TextMeshProUGUI txt)
    {
        if (txt == null) yield break;
        float t = 0f;
        while (t < velocidadFade)
        {
            t += Time.deltaTime;
            txt.alpha = Mathf.Clamp01(t / velocidadFade);
            yield return null;
        }
        txt.alpha = 1f;
    }

    // ─── Fade out ─────────────────────────────────────────────────────────────
    IEnumerator Ocultar(TextMeshProUGUI txt, float dur)
    {
        if (txt == null) yield break;
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            txt.alpha = Mathf.Clamp01(1f - (t / dur));
            yield return null;
        }
        txt.alpha = 0f;
    }

    // ─── Estado dominante ─────────────────────────────────────────────────────
    string GetEstado(GameManager gm)
    {
        int n = gm.negacion, d = gm.duda, a = gm.aceptacion;
        if (n > d && n > a) return "Negacion";
        if (a > n && a > d) return "Aceptacion";
        if (d > n && d > a) return "Duda";
        return "Equilibrio";
    }

    string GetTitulo(string e) => e switch
    {
        "Negacion" => "No quisiste ver.",
        "Duda" => "Lo sentiste, pero seguiste.",
        "Aceptacion" => "Ya lo sabías.",
        _ => "No te decidiste por ninguna versión."
    };

    string GetCuerpo(string e) => e switch
    {
        "Negacion" =>
            "Martín lleva seis meses en ese apartamento\n" +
            "eligiendo la explicación más cómoda.\n\n" +
            "Tú hiciste lo mismo.",
        "Duda" =>
            "Algo en ti notó que las cosas no cuadraban.\n\n" +
            "Pero igual seguiste adelante.\n" +
            "Igual que Martín.",
        "Aceptacion" =>
            "Viste lo que Martín no pudo ver en seis meses.\n\n" +
            "Y aun así seguiste aquí,\n" +
            "esperando una respuesta que el juego\n" +
            "todavía no te va a dar.",
        _ =>
            "Tuviste la información\n" +
            "y no te decidiste por ninguna versión.\n\n" +
            "Elena diría que eso también es una elección."
    };

    string GetCita(string e) => e switch
    {
        "Negacion" =>
            "\"El problema no es que no lo sepas.\n" +
            "El problema es que no quieres saber.\"\n\n— Elena",
        "Duda" =>
            "\"Llevas mucho tiempo buscando algo.\n" +
            "Lo que estás buscando no es lo que crees.\"\n\n— Elena",
        "Aceptacion" =>
            "\"Cuando estés listo para saberlo,\n" +
            "ya lo vas a saber.\"\n\n— Elena",
        _ =>
            "\"¿Cuánto tiempo crees que llevas aquí?\"\n\n— Elena"
    };

    string GetEstadoHUD(GameManager gm, string e)
    {
        string nombre = e switch
        {
            "Negacion" => "NEGACIÓN",
            "Aceptacion" => "ACEPTACIÓN",
            "Duda" => "DUDA",
            _ => "EQUILIBRIO"
        };
        return $"Estado dominante: {nombre}\n\n" +
               $"Negación       {gm.negacion}\n" +
               $"Duda           {gm.duda}\n" +
               $"Aceptación     {gm.aceptacion}";
    }
}