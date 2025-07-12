using System.Collections.Generic;
using UnityEngine;

public class FemaleDupontHandler : MonoBehaviour
{
    public Transform hoveringHeaderPin = null;

    public static List<FemaleDupontHandler> All = new List<FemaleDupontHandler>();

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    void OnMouseUp()
    {
        if (hoveringHeaderPin != null)
        {
            Debug.Log($"Dupont [{this.gameObject.name}] was released over male heder pin [{hoveringHeaderPin.name}]");
            Debug.Log("Connection is allowed");
        }
        else
        {
            Debug.Log("Connection is NOT allowed");
        }

        hoveringHeaderPin = null;
    }
}
