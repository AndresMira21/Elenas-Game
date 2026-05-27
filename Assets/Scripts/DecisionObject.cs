using UnityEngine;
using System.Collections.Generic;

public class DecisionObject : MonoBehaviour
{
    [System.Serializable]
    public class DecisionPorDia
    {
        public int dia;
        [TextArea] public string title;
        public List<DecisionData> options = new List<DecisionData>();
        public bool oneUseOnly = true;
        [HideInInspector] public bool used = false;
    }

    [Header("Decisiones por día")]
    public List<DecisionPorDia> decisionesPorDia = new List<DecisionPorDia>();

    bool CanUse()
    {
        return GameManager.Instance != null;
    }

    public bool Inspect()
    {
        if (!CanUse()) return false;

        int day = GameManager.Instance.currentDay;

        DecisionPorDia entry = decisionesPorDia.Find(d => d.dia == day);

        if (entry == null) return false;
        if (entry.oneUseOnly && entry.used) return false;

        GameManager.Instance.RegisterExploration();

        var runtime = new List<DecisionOption>();
        foreach (var opt in entry.options)
        {
            var optCopy = opt;
            runtime.Add(new DecisionOption(opt.text, () => Execute(optCopy)));
        }

        DecisionManager.Instance.Show(
            entry.title,
            runtime,
            () =>
            {
                entry.used = true;
                GameManager.Instance.decisionMade = true;
            }
        );

        return true;
    }

    void Execute(DecisionData opt)
    {
        if (opt.type == DecisionType.Negacion) GameManager.Instance.AddNegacion(opt.value);
        if (opt.type == DecisionType.Duda) GameManager.Instance.AddDuda(opt.value);
        if (opt.type == DecisionType.Aceptacion) GameManager.Instance.AddAceptacion(opt.value);
        Debug.Log(opt.text);
    }
}