using UnityEngine;
using System.Collections.Generic;

public class DecisionObject : MonoBehaviour
{
    [TextArea]
    public string title;

    public List<DecisionData> options =
        new List<DecisionData>();

    [Header("Días activos")]
    public int activeFromDay = 1;
    public int activeUntilDay = 999;

    [Header("Uso")]
    public bool oneUseOnly = true;

    bool used = false;

    bool CanUse()
    {
        if (GameManager.Instance == null)
            return false;

        int day = GameManager.Instance.currentDay;

        return day >= activeFromDay &&
               day <= activeUntilDay;
    }

    public bool Inspect()
    {
        if (!CanUse())
            return false;

        if (oneUseOnly && used)
            return false;

        GameManager.Instance.RegisterExploration();

        List<DecisionOption> runtime =
            new List<DecisionOption>();

        foreach (var opt in options)
        {
            runtime.Add(
                new DecisionOption(opt.text, () =>
                {
                    Execute(opt);
                })
            );
        }

        DecisionManager.Instance.Show(
            title,
            runtime,
            () =>
            {
                used = true;
                GameManager.Instance.decisionMade = true;
            });

        return true;
    }

    void Execute(DecisionData opt)
    {
        if (opt.type == DecisionType.Negacion)
            GameManager.Instance.AddNegacion(opt.value);

        if (opt.type == DecisionType.Duda)
            GameManager.Instance.AddDuda(opt.value);

        if (opt.type == DecisionType.Aceptacion)
            GameManager.Instance.AddAceptacion(opt.value);

        Debug.Log(opt.text);
    }
}