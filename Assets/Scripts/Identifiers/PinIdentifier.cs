using UnityEngine;

public enum PinType
{
    PIN_MOTHER,
    PIN_FATHER
}

public class PinIdentifier
{
    public string Name;
    public PinType Type;

    public PinIdentifier(string name, PinType type)
    {
        Name = name;
        Type = type;
    }
}
