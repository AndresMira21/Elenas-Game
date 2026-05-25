using UnityEngine;
using System.Collections.Generic;

public class DecisionObject : MonoBehaviour
{
    [TextArea]
    public string title;

    public List<DecisionData> options = new List<DecisionData>();

    bool used = false;

    public void Inspect()
    {
        if (used) return;

        List<DecisionOption> runtimeOptions = new List<DecisionOption>();

        foreach (var opt in options)
        {
            runtimeOptions.Add(new DecisionOption(opt.text, () =>
            {
                Execute(opt);
            }));
        }

        DecisionManager.Instance.Show(
            title,
            runtimeOptions,
            () =>
            {
                used = true;
            }
        );
    }

    void Execute(DecisionData opt)
    {
        if (opt.type == DecisionType.Negacion)
        {
            GameManager.Instance.AddNegacion(opt.value);
        }
        else if (opt.type == DecisionType.Duda)
        {
            GameManager.Instance.AddDuda(opt.value);
        }
        else if (opt.type == DecisionType.Aceptacion)
        {
            GameManager.Instance.AddAceptacion(opt.value);
        }

        Debug.Log("📌 " + opt.text + " → " + opt.type + " +" + opt.value);
    }
}