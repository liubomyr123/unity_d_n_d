using UnityEngine;

public class BoardObjectIdentifier : MonoBehaviour
{
    public string Id;
    public string Type;
    //public bool IsOccupied;

    public void SetId(string id, string type)
    {
        Id = id;
        Type = type;
    }
}
