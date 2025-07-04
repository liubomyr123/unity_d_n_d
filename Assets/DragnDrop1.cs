//using UnityEngine;

//public class DragnDrop1 : MonoBehaviour
//{
//    private Vector3 offset;
//    private Plane dragPlane;

//    void Start()
//    {
//        Rigidbody rb = GetComponent<Rigidbody>();
//        if (rb != null)
//        {
//            rb.useGravity = false;
//            rb.isKinematic = true;
//        }
//    }

//    private void OnMouseDown()
//    {
//        // �������, ��������������� �� ������, ��������� ����� ��'���
//        dragPlane = new Plane(Camera.main.transform.forward * -1, transform.position);

//        // ��������� ������ � �����
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        // ��������� ����� �������� ������� � ��������
//        if (dragPlane.Raycast(ray, out float enter))
//        {
//            Vector3 hitPoint = ray.GetPoint(enter);
//            offset = transform.position - hitPoint;
//        }
//    }

//    private void OnMouseDrag()
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        if (dragPlane.Raycast(ray, out float enter))
//        {
//            Vector3 hitPoint = ray.GetPoint(enter);
//            transform.position = hitPoint + offset;
//        }
//    }
//}


using UnityEngine;

public class DragnDrop1 : MonoBehaviour
{
    private float distanceFromCamera;
    private Vector3 offset;

    void Start()
    {
        if (TryGetComponent<Rigidbody>(out var rb))
        {
            rb.useGravity = false;
            rb.isKinematic = true;
        }
    }

    void Update()
    {
        // ���� ������� ����� ���� Shift ����������
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            float scroll = Input.mouseScrollDelta.y;

            if (scroll != 0f)
            {
                distanceFromCamera -= scroll * 0.5f;
                distanceFromCamera = Mathf.Clamp(distanceFromCamera, 1f, 100f);
            }
        }
    }

    void OnMouseDown()
    {
        distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);

        // �������� ����� �� ������ � 3D �� ������ �������
        Vector3 worldPos = GetWorldPositionAtDistance(distanceFromCamera);
        offset = transform.position - worldPos;
    }

    void OnMouseDrag()
    {
        Vector3 worldPos = GetWorldPositionAtDistance(distanceFromCamera);
        transform.position = worldPos + offset;
    }

    Vector3 GetWorldPositionAtDistance(float distance)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction * distance;
    }
}
