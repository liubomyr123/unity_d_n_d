using System;
using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minDistance = 2f;
    [SerializeField] private float maxDistance = 50f;
    public float panSpeed = 0.5f;
    public float rotationSpeed = 5f;

    // ����� �����/����
    private float pitch = 0f;
    // ������� ����/������
    private float yaw = 0f;


    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        pitch = angles.x;
        yaw = angles.y;
    }

    private void Update()
    {
        float scroll_value = Input.GetAxis("Mouse ScrollWheel");

        // ���������� �����, ���� ���������� Shift (��������������� ��� ������� ��'����)
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            scroll_value = 0f;
        }

        if (Mathf.Abs(scroll_value) > 0.01f)
        {
            // �������� ���� �������� ������ ������ �� Z, �������� ����. ���� ��������� -0.1, �� �����. ���� +0.1, ��� ������.
            Vector3 direction = transform.forward;
            Vector3 newPosition = transform.position + direction * scroll_value * zoomSpeed;

            // ³������ �� ���� ������� ������ �� ����� (0,0,0).
            float distance = Vector3.Distance(newPosition, Vector3.zero);
            if (distance >= minDistance && distance <= maxDistance)
            {
                transform.position = newPosition;
            }
        }


        if (Input.GetMouseButton(2))
        {
            // ������ ��� ����
            // ³䒺���� ���� - ��� ������ ���� (��� ��� �� ���� �������� �� ����� ������)
            float moveX = -Input.GetAxis("Mouse X") * panSpeed;
            float moveY = -Input.GetAxis("Mouse Y") * panSpeed;

            // ��������� ������
            // ��� �� �������� ���� X � Y
            transform.Translate(moveX, moveY, 0);
        }

        if (Input.GetMouseButton(1))  // ����� ������ ���� ����������
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;

            // ��������� ������ ��� ������ �� ������������
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
