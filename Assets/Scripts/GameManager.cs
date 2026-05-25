using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int negation = 0;
    public int acceptance = 0;
    public int doubt = 0;
    public int currentNight = 1;

    void Awake()
    {
        Instance = this;
    }

    public void AddNegation(int amount) { negation += amount; }
    public void AddAcceptance(int amount) { acceptance += amount; }
    public void AddDoubt(int amount) { doubt += amount; }
}