using UnityEngine;

public class DragUIElementIdentifier : MonoBehaviour
{
    public string Id;
    public string Type;

    public void SetId(string id, string type)
    {
        Id = id;
        Type = type;
    }
}
