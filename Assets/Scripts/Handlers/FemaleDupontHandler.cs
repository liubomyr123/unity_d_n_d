using System.Collections.Generic;
using UnityEngine;

public class FemaleDupontHandler : MonoBehaviour
{
    public Transform hoveringHeaderPin = null;
    private bool is_attached = false;

    public static List<FemaleDupontHandler> All = new List<FemaleDupontHandler>();

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }

    void OnMouseDown()
    {
        if (is_attached)
        {
            Detach();
        }
    }

    private void Attach()
    {
        if (hoveringHeaderPin == null) return;

        transform.SetParent(hoveringHeaderPin);

        Vector3 offset = new Vector3(0.01f, 1.96f, 0.01f);
        transform.position = hoveringHeaderPin.transform.position + offset;
        is_attached = true;
    }

    public void Detach()
    {
        transform.SetParent(null);
        is_attached = false;
    }

    public bool IsAttached()
    {
       return is_attached;
    }

    void OnMouseUp()
    {
        if (hoveringHeaderPin != null)
        {
            Debug.Log($"Dupont [{this.gameObject.name}] was released over male heder pin [{hoveringHeaderPin.name}]");
            Debug.Log("Connection is allowed");
            Attach();
        }
        else
        {
            Debug.Log("Connection is NOT allowed");
        }

        hoveringHeaderPin = null;
    }
}
