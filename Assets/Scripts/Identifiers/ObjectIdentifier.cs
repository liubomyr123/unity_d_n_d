using UnityEngine;
public enum ObjectType
{
    Cube,
    Sphere,
    Chair
}
public class ObjectIdentifier : MonoBehaviour
{
    public string Id;
    public string Type;

    public void SetId(string id, string type)
    {
        Type = type;
        Id = id;
    }
}
