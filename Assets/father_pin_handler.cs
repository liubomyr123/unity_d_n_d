using System.Collections.Generic;
using UnityEngine;

public class father_pin_handler : MonoBehaviour
{
    public bool IsHoveringOverValidPin { get; set; } = false;


    public static List<father_pin_handler> All = new List<father_pin_handler>();

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
            Debug.Log("Mouse released over mother pin!");
            Debug.Log("����� ��'������ � �� ���� ��� ���������� ���� �� �������!");
            // ��� ����� ��������� ����� �'�������, ���� �������, ����
        }
        else
        {
            Debug.Log("�� ��� ���������� ���� � ����� �� �'������");
        }

        // ������� ���� ��������
        IsHoveringOverValidPin = false;
    }
}
