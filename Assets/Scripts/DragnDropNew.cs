using UnityEngine;

public class DragnDropNew : MonoBehaviour
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
        // «м≥на глибини т≥льки €кщо Shift утримуЇтьс€
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

        // ќтримуЇмо точку п≥д мишкою у 3D на певн≥й глибин≥
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
