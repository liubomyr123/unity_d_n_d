using UnityEngine;

public class DragnDrop : MonoBehaviour
{
    private Vector3 offset;
    private Plane dragPlane;
    private bool is_attached = false;
    private PinHandler attachedPin = null;
    [SerializeField] private Material boxMaterail;
    
    void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (boxMaterail != null)
        {
            boxMaterail = new Material(boxMaterail);
            GetComponent<Renderer>().material = boxMaterail;
        }

        UpdateBoxMateial(Color.green);
    }

    private void UpdateBoxMateial(Color new_color)
    {
        if (boxMaterail != null)
        {
            boxMaterail.color = new_color;
        }
    }

    private void OnMouseDown()
    {
        if (is_attached && attachedPin != null)
        {
            attachedPin.FreePin();  // ��������� ��
            attachedPin = null;
        }

        is_attached = false;
        UpdateBoxMateial(Color.green);
        // �������, ��������������� �� ������, ��������� ����� ��'���
        dragPlane = new Plane(Camera.main.transform.forward * -1, transform.position);

        // ��������� ������ � �����
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // ��������� ����� �������� ������� � ��������
        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            offset = transform.position - hitPoint;
        }
    }

    private void OnMouseDrag()
    {
        if (is_attached) return;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (dragPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            transform.position = hitPoint + offset;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!is_attached)
        {
            PinHandler pin = other.GetComponent<PinHandler>();
            if (pin != null && !pin.IsOccupied())
            {
                Attach(pin.transform);
                attachedPin = pin;
                pin.SetOccupied(true);
            }
        }
    }

    private BoxIdentity GetBoxIdentity()
    {
        return GetComponent<BoxIdentity>();
    }

    public void Attach(Transform pinTransform)
    {
        is_attached = true;
        UpdateBoxMateial(Color.red);
        // ������� ����� �� 0.5
        Vector3 offset = new Vector3(0f, 0.5f, 0f);
        transform.position = pinTransform.position + offset;
        transform.rotation = pinTransform.rotation;
    }
}