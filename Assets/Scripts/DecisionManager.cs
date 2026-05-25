using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance;
    public static bool IsActive = false;
    Action onFinish;

    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text[] optionTexts;

    int index = 0;
    List<DecisionOption> options = new List<DecisionOption>();

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    void Update()
    {
        if (!IsActive) return;

        if (Input.GetKeyDown(KeyCode.W))
        {
            index--;
            if (index < 0) index = options.Count - 1;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            index++;
            if (index >= options.Count) index = 0;
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SelectOption();
        }
    }

    public void Show(string title, List<DecisionOption> newOptions, Action finishCallback)
    {
        IsActive = true;

        options = newOptions;
        index = 0;

        onFinish = finishCallback;

        panel.SetActive(true);

        PlayerMovement.canMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        titleText.text = title;

        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < optionTexts.Length; i++)
        {
            if (i < options.Count)
            {
                optionTexts[i].gameObject.SetActive(true);

                string arrow = (i == index) ? "▶ " : "   ";
                optionTexts[i].text = arrow + options[i].text;
            }
            else
            {
                optionTexts[i].gameObject.SetActive(false);
            }
        }
    }

    void SelectOption()
    {
        if (options.Count == 0) return;

        Debug.Log("Elegiste: " + options[index].text);

        options[index].onSelect?.Invoke();

        onFinish?.Invoke(); // 🔥 bloquea objeto

        Hide();
    }

    public void Hide()
    {
        IsActive = false;
        panel.SetActive(false);

        PlayerMovement.canMove = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}

[Serializable]
public class DecisionOption
{
    public string text;
    public Action onSelect;

    public DecisionOption(string t, Action a)
    {
        text = t;
        onSelect = a;
    }
}