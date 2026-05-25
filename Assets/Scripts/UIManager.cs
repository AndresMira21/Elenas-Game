using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject panel;
    public TMP_Text text;

    void Start()
    {
        Hide();
    }

    public void Show(string msg)
    {
        panel.SetActive(true);
        text.gameObject.SetActive(true);
        text.text = msg;
    }

    public void Hide()
    {
        panel.SetActive(false);
        text.gameObject.SetActive(false);
    }

    public void SetText(string msg)
    {
        text.text = msg;
    }
}