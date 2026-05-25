using System.Collections;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public UIManager ui;

    void Awake()
    {
        Instance = this;
    }

    public void ShowThought(string message, float time = 3f)
    {
        StartCoroutine(Routine(message, time));
    }

    IEnumerator Routine(string message, float time)
    {
        ui.Show(message);
        yield return new WaitForSeconds(time);
        ui.Hide();
    }
}