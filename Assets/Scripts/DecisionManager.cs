using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance;

    public GameObject panel;
    public TMP_Text titleText;
    public TMP_Text[] optionTexts;

    List<DecisionOption> options = new List<DecisionOption>();
    int index;
    Action onFinish;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void Show(string title, List<DecisionOption> newOptions, Action finish)
    {
        panel.SetActive(true);

        options = newOptions;
        index = 0;
        onFinish = finish;

        titleText.text = title;

        PlayerMovement.canMove = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateUI();
    }

    void Update()
    {
        if (!panel.activeSelf) return;

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
            Select();
        }
    }

    void UpdateUI()
    {
        for (int i = 0; i < optionTexts.Length; i++)
        {
            if (i < options.Count)
            {
                optionTexts[i].text =
                    (i == index ? "▶ " : "  ") + options[i].text;
            }
        }
    }

    void Select()
    {
        options[index].onSelect?.Invoke();
        onFinish?.Invoke();

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