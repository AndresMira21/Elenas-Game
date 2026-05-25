using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;

    void Start()
    {
        panel.SetActive(false);
    }

    public void Show(string msg)
    {
        panel.SetActive(true);
        text.text = msg;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}