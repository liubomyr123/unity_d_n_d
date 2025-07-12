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

    // Нахил вверх/вниз
    private float pitch = 0f;
    // Поворот вліво/вправо
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

        // Ігнорувати скрол, якщо утримується Shift (використовується для глибини об'єкта)
        if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            scroll_value = 0f;
        }

        if (Mathf.Abs(scroll_value) > 0.01f)
        {
            // Напрямок куди дивиться камера вздовж осі Z, напрямку зуму. Якщо наприклад -0.1, то назад. Якщо +0.1, тоді вперед.
            Vector3 direction = transform.forward;
            Vector3 newPosition = transform.position + direction * scroll_value * zoomSpeed;

            // Відстань від нової позиції камери до точки (0,0,0).
            float distance = Vector3.Distance(newPosition, Vector3.zero);
            if (distance >= minDistance && distance <= maxDistance)
            {
                transform.position = newPosition;
            }
        }


        if (Input.GetMouseButton(2))
        {
            // Читаємо рух миші
            // Від’ємний знак - для інверсії руху (щоб рух по миші співпадав із рухом камери)
            float moveX = -Input.GetAxis("Mouse X") * panSpeed;
            float moveY = -Input.GetAxis("Mouse Y") * panSpeed;

            // Переміщуємо камеру
            // Рух по локальній осях X і Y
            transform.Translate(moveX, moveY, 0);
        }

        if (Input.GetMouseButton(1))  // права кнопка миші утримується
        {
            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            yaw += mouseX * rotationSpeed;
            pitch -= mouseY * rotationSpeed;

            // Обмеження нахилу щоб камера не перекидалася
            pitch = Mathf.Clamp(pitch, -80f, 80f);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0f);
        }
    }
}
