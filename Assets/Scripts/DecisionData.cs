using System;

[Serializable]
public class DecisionData
{
    public string text;
    public DecisionType type;
    public int value;
}

public enum DecisionType
{
    Negacion,
    Duda,
    Aceptacion
}