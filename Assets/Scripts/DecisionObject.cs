using UnityEngine;
using System.Collections.Generic;

public class DecisionObject : MonoBehaviour
{
    [TextArea]
    public string title;

    public List<DecisionData> options = new List<DecisionData>();

    public int activeFromDay = 1;

    bool used = false;

    bool CanUse()
    {
        return GameManager.Instance.currentDay >= activeFromDay;
    }

    public void Inspect()
    {
        if (!CanUse())
        {
            Debug.Log("No disponible hoy");
            return;
        }

        if (used) return;

        GameManager.Instance.RegisterExploration();

        List<DecisionOption> runtime = new List<DecisionOption>();

        foreach (var opt in options)
        {
            runtime.Add(new DecisionOption(opt.text, () =>
            {
                Execute(opt);
            }));
        }

        DecisionManager.Instance.Show(title, runtime, () =>
        {
            used = true;
            GameManager.Instance.decisionMade = true;
        });
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