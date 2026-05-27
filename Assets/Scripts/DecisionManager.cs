using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DecisionManager : MonoBehaviour
{
    public static DecisionManager Instance;

    //  Estado del menú
    public static bool IsActive = false;

    //  Evita reabrir decisiones instantáneamente
    public static bool BlockInteraction = false;

    [Header("UI")]
    public GameObject panel;

    public TMP_Text titleText;

    public TMP_Text[] optionTexts;

    List<DecisionOption> options =
        new List<DecisionOption>();

    int index;

    Action onFinish;

    void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    public void Show(
        string title,
        List<DecisionOption> newOptions,
        Action finish)
    {
        IsActive = true;

        panel.SetActive(true);

        options = newOptions;

        index = 0;

        onFinish = finish;

        titleText.text = title;

        // bloquear jugador
        PlayerMovement.canMove = false;
        PlayerMovement.canLook = false;

        // mostrar cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateUI();
    }

    void Update()
    {
        if (!panel.activeSelf)
            return;

        // subir
        if (Input.GetKeyDown(KeyCode.W))
        {
            index--;

            if (index < 0)
                index = options.Count - 1;

            UpdateUI();
        }

        // bajar
        if (Input.GetKeyDown(KeyCode.S))
        {
            index++;

            if (index >= options.Count)
                index = 0;

            UpdateUI();
        }

        // seleccionar
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
                optionTexts[i].gameObject.SetActive(true);

                //  usamos > para evitar error TMP
                optionTexts[i].text =
                    (i == index ? "> " : "  ")
                    + options[i].text;
            }
            else
            {
                optionTexts[i].gameObject.SetActive(false);
            }
        }
    }

    void Select()
    {
        // ejecutar acción
        options[index].onSelect?.Invoke();

        // callback final
        onFinish?.Invoke();

        Hide();
    }

    public void Hide()
    {
        IsActive = false;

        panel.SetActive(false);

        // devolver control
        PlayerMovement.canMove = true;
        PlayerMovement.canLook = true;

        // ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // evitar reapertura instantánea
        StartCoroutine(UnlockInteraction());
    }

    IEnumerator UnlockInteraction()
    {
        BlockInteraction = true;

        yield return new WaitForSeconds(0.2f);

        BlockInteraction = false;
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