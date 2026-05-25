using System;
using UnityEngine;

[Serializable]
public class DecisionData
{
    public string text;

    public DecisionType type;

    public int value = 1;
}

public enum DecisionType
{
    Negacion,
    Duda,
    Aceptacion
}