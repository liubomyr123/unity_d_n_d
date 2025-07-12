using UnityEngine;

public enum HeaderPinType
{
    MaleHeaderPin,
    FemaleHeaderPin
}

public class HeaderPinIdentifier
{
    public string Name;
    public HeaderPinType Type;

    public HeaderPinIdentifier(string name, HeaderPinType type)
    {
        Name = name;
        Type = type;
    }
}
