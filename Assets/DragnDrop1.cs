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
//        // Площина, перпендикулярна до камери, проходить через об'єкт
//        dragPlane = new Plane(Camera.main.transform.forward * -1, transform.position);

//        // Створюємо промінь з мишки
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//        // Знаходимо точку перетину променя з площиною
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
        // Зміна глибини тільки якщо Shift утримується
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
        ObjectIdentifier id = GetComponent<ObjectIdentifier>();
        if (id != null)
        {
            Debug.Log("Clicked on object with type: " + id.Type);
            Debug.Log("Clicked on object with id: " + id.Id);
        }

        distanceFromCamera = Vector3.Distance(Camera.main.transform.position, transform.position);

        // Отримуємо точку під мишкою у 3D на певній глибині
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
