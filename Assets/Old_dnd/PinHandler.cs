using UnityEngine;

public class PinHandler : MonoBehaviour
{
    private bool isPinOccupied = false;

    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

    }

    public void FreePin()
    {
        isPinOccupied = false;
    }

    public bool IsOccupied()
    {
        return isPinOccupied;
    }

    public void SetOccupied(bool value)
    {
        isPinOccupied = value;
    }
}
