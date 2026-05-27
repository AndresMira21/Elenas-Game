using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Día")]
    public int currentDay = 1;

    [Header("Progreso")]
    public int objectsExplored = 0;
    public bool decisionMade = false;

    [Header("Emociones")]
    public int negacion;
    public int duda;
    public int aceptacion;

    void Awake()
    {
        Debug.Log("GAME MANAGER FUNCIONANDO");
        Instance = this;
    }

    public void RegisterExploration()
    {
        objectsExplored++;
        Debug.Log("Exploración: " + objectsExplored);
    }

    public void AddNegacion(int v)
    {
        negacion += v;
        decisionMade = true;
        Debug.Log("Negación +" + v);
    }

    public void AddDuda(int v)
    {
        duda += v;
        decisionMade = true;
        Debug.Log("Duda +" + v);
    }

    public void AddAceptacion(int v)
    {
        aceptacion += v;
        decisionMade = true;
        Debug.Log("Aceptación +" + v);
    }

    public bool CanSleep()
    {
        return objectsExplored >= 3 && decisionMade;
    }

    public void Sleep()
    {
        Debug.Log("😴 Durmiendo...");
        currentDay++;
        Debug.Log("📅 Día: " + currentDay);
        objectsExplored = 0;
        decisionMade = false;

        if (currentDay == 2)
        ElenaEventos.Instance.ActivarYAparece();
        else if (currentDay == 3)
        {
            ElenaEventos.Instance.PrepararNivel3();
        }
        else if (currentDay == 4)
        {
            ElenaEventos.Instance.ActivarNivel4();
        }
        else if (currentDay == 5)
        {
            ElenaEventos.Instance.PrepararNivel5();
        }
    }
}