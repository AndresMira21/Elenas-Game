using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    // =========================
    // ESTADO EMOCIONAL
    // =========================
    public int negacion;
    public int duda;
    public int aceptacion;

    // =========================
    // PROGRESO
    // =========================
    public int objectsExplored = 0;
    public bool decisionMade = false;

    void Awake()
    {
        Instance = this;
    }

    // =========================
    // DECISIONES + LOG
    // =========================
    public void AddNegacion(int v)
    {
        negacion += v;
        decisionMade = true;
        Debug.Log("📌 Elena - Negación + " + v + " | Total: " + negacion);
    }

    public void AddDuda(int v)
    {
        duda += v;
        decisionMade = true;
        Debug.Log("📌 Elena - Duda + " + v + " | Total: " + duda);
    }

    public void AddAceptacion(int v)
    {
        aceptacion += v;
        decisionMade = true;
        Debug.Log("📌 Elena - Aceptación + " + v + " | Total: " + aceptacion);
    }

    // =========================
    // EXPLORACIÓN
    // =========================
    public void RegisterExploration()
    {
        objectsExplored++;
        Debug.Log("🔍 Objeto explorado | Total: " + objectsExplored);
    }

    // =========================
    // REGLA DE SUEÑO (IMPORTANTE)
    // =========================
    public bool CanSleep()
    {
        return objectsExplored >= 3 && decisionMade;
    }

    public void Sleep()
    {
        Debug.Log("😴 Durmiendo... avanzando día");

        // reset de progreso por día
        objectsExplored = 0;
        decisionMade = false;

        Debug.Log("🌅 Nuevo día iniciado");
    }
}