using System.Collections.Generic;
using UnityEngine;

public class mother_pin_handler : MonoBehaviour
{
    public bool IsHoveringOverValidPin { get; set; } = false;

    public static List<mother_pin_handler> All = new List<mother_pin_handler>();

    void OnEnable()
    {
        All.Add(this);
    }

    void OnDisable()
    {
        All.Remove(this);
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        if (IsHoveringOverValidPin)
        {
            Debug.Log("Mouse released over father pin!");
            Debug.Log("Можна під'єднати — ми були над правильним піном на ардуінці!");
            // Тут можеш викликати метод з'єднання, зміни кольору, інше
        }
        else
        {
            Debug.Log("Не над правильним піном — нічого не з'єднуємо");
        }

        // Скидаємо після перевірки
        IsHoveringOverValidPin = false;
    }
}
