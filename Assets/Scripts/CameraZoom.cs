using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float panSpeed = 0.5f;
    [SerializeField] private float orbitSpeed = 5f;

    // Нахил вверх/вниз
    private float pitch = 0f;
    // Поворот вліво/вправо
    private float yaw = 0f;

    private Vector3 lastMousePosition;

    void Start()
    {
        if (Camera.main != null)
        {
            if (Camera.main.gameObject.GetComponent<AxisIndicator>() == null)
            {
                Camera.main.gameObject.AddComponent<AxisIndicator>();
            }
        }
        else
        {
            Debug.LogWarning("Main Camera not found!");
        }

        InitializeRotationAngles();
    }

    private void InitializeRotationAngles()
    {
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;
    }

    private void Update()
    {
        HandleZoom();
        HandleOrbitAndPan();
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Ignore zoom if Shift is held (used for pan)
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            scroll = 0f;
        }

        if (Mathf.Abs(scroll) > 0.01f)
        {
            Vector3 direction = transform.forward;
            Vector3 newPosition = transform.position + direction * scroll * zoomSpeed;

            float distance = Vector3.Distance(newPosition, Vector3.zero);
            if (distance >= minDistance && distance <= maxDistance)
            {
                transform.position = newPosition;
            }
        }
    }

    private void HandleOrbitAndPan()
    {
        // Save last mouse position
        if (Input.GetMouseButtonDown(2)) // MMB
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2)) // MMB
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;

            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // SHIFT + MMB → pan
                Vector3 move = new Vector3(delta.x * -1f * panSpeed * Time.deltaTime, -delta.y * panSpeed * Time.deltaTime, 0);
                transform.Translate(move, Space.Self);
            }
            else
            {
                // MMB → orbit around (0,0,0)
                float rotX = -delta.y * orbitSpeed * Time.deltaTime;
                float rotY = delta.x * orbitSpeed * Time.deltaTime;

                pitch += rotX;
                yaw += rotY;
                pitch = Mathf.Clamp(pitch, -80f, 80f);

                // Rotate around the origin
                transform.RotateAround(Vector3.zero, Vector3.up, rotY);
                transform.RotateAround(Vector3.zero, transform.right, rotX);
            }
        }
    }
}
