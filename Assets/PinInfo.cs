using UnityEngine;

public enum PinType
{
    PIN_MOTHER,
    PIN_FATHER
}

public class PinInfo
{
    public string Name;
    public PinType Type;

    public PinInfo(string name, PinType type)
    {
        Name = name;
        Type = type;
    }
}
