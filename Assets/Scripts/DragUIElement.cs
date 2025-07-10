using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragUIElement : MonoBehaviour, 
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    // this is the prefab that you want to create when you drag and drop from the UI element.
    [SerializeField] GameObject PrefabToInstatiate;

    // Cache the reference to the UI drag elemnt so that we can apply
    // transformations to it.
    [SerializeField] RectTransform UIDragElement;

    // Cache the reference to the canvas.
    [SerializeField] RectTransform Canvas;

    // Private data to store the pointer positions.
    private Vector2 mOriginalLocalPointerPosition;
    private Vector3 mOriginalPanelLocalPosition;
    private Vector2 mOriginalPosition;
    
    
    void Start()
    {
        // We store the initial position of the draggble ui item
        mOriginalPosition = UIDragElement.localPosition;
    }

    public void OnBeginDrag(PointerEventData data)
    {
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out mOriginalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out localPointerPosition))
        {
            Vector3 offsetToOriginal = localPointerPosition - mOriginalLocalPointerPosition;

            UIDragElement.localPosition = mOriginalPanelLocalPosition + offsetToOriginal;
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        StartCoroutine(Coroutine_MouseUIElenment(
            UIDragElement,
            mOriginalPosition,
            0.5f));

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 spawnPoint;

        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

        if (groundPlane.Raycast(ray, out float enter))
        {
            spawnPoint = ray.GetPoint(enter);
        }
        else
        {
            // fallback, якщо не перетнуло (малоймовірно)
            spawnPoint = ray.origin + ray.direction * 5f;
        }

        CreateObject(spawnPoint);
    }

    void CreateObject(Vector3 pos)
    {
        if (PrefabToInstatiate == null)
        {
            Debug.Log("No prefab");
            return;
        }
        string type = PrefabToInstatiate.name;

        //Quaternion rotation = type == "Arduino_Mother"
        //    ? Quaternion.Euler(180f, 0f, 90f)
        //    : Quaternion.identity;

        GameObject obj = Instantiate(
            PrefabToInstatiate,
            pos,
            Quaternion.identity);

        AddCollidersToChildren(obj);

        string uniqueId = System.Guid.NewGuid().ToString(); // або інший генератор

        ObjectIdentifier idComp = obj.GetComponent<ObjectIdentifier>();
        if (idComp == null)
        {
            idComp = obj.AddComponent<ObjectIdentifier>();
        }
        idComp.SetId(uniqueId, type);

        switch (type)
        {
            case "Arduino_Father":
                Debug.Log("It is father element. Init father_pin_handler");
                if (!obj.TryGetComponent<father_pin_handler>(out _))
                {
                    FitBoxColliderToChildren(obj);
                    obj.AddComponent<father_pin_handler>();
                }
                break;

            case "Arduino_Mother":
                Debug.Log("It is mother element. Init mother_pin_handler");
                if (!obj.TryGetComponent<mother_pin_handler>(out _))
                {
                    FitBoxColliderToChildren(obj);
                    obj.AddComponent<mother_pin_handler>();
                }
                break;

            case "Arduino_Uno_10":
                Debug.Log("It is uno element. Init uno_handler");
                if (!obj.TryGetComponent<uno_handler>(out _))
                {
                    FitBoxColliderToChildren(obj);
                    obj.AddComponent<uno_handler>();
                }
                break;

            default:
                Debug.Log("Element was not found");
                break;
        }

        // Додаємо Rigidbody, якщо ще немає
        if (!obj.TryGetComponent<Rigidbody>(out _))
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();

            // За бажанням: налаштовуємо Rigidbody
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (!obj.TryGetComponent<DragnDropNew>(out _))
        {
            FitBoxColliderToChildren(obj);
            obj.AddComponent<DragnDropNew>();
        }

        // Додаємо BoxCollider, якщо ще немає
        if (!obj.TryGetComponent<BoxCollider>(out _))
        {
            obj.AddComponent<BoxCollider>();
        }
    }

    IEnumerator Coroutine_MouseUIElenment(
        RectTransform r,
        Vector2 targetPosition,
        float duration = 0.1f)
    {
        float elaspedTime = 0;
        Vector2 startPos = r.localPosition;
        while(elaspedTime < duration)
        {
            r.localPosition = Vector2.Lerp(
                startPos,
                targetPosition,
                (elaspedTime / duration));
            elaspedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        r.localPosition = targetPosition;
    }

    void FitBoxColliderToChildren(GameObject parent)
    {
        // Створюємо Bounds із позиції об'єкта
        Bounds bounds = new Bounds(parent.transform.position, Vector3.zero);
        bool foundAny = false;

        // Проходимось по всіх MeshRenderer-ах дітей
        foreach (Renderer r in parent.GetComponentsInChildren<Renderer>())
        {
            if (!foundAny)
            {
                bounds = r.bounds;
                foundAny = true;
            }
            else
            {
                bounds.Encapsulate(r.bounds);
            }
        }

        if (foundAny)
        {
            // Додаємо BoxCollider, якщо треба
            if (!parent.TryGetComponent<BoxCollider>(out var collider))
            {
                collider = parent.AddComponent<BoxCollider>();
            }

            // Переводимо світові bounds у локальні координати
            Vector3 localCenter = parent.transform.InverseTransformPoint(bounds.center);
            Vector3 localSize = parent.transform.InverseTransformVector(bounds.size);

            collider.center = localCenter;
            collider.size = localSize;
        }
    }
        void AddCollidersToChildren(GameObject parent)
    {
        foreach (Transform child in parent.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.GetComponent<Collider>() == null)
            {
                Renderer r = child.GetComponent<Renderer>();
                if (r != null)
                {
                    // Додаємо BoxCollider тільки якщо є Renderer
                    child.gameObject.AddComponent<BoxCollider>();
                }
            }
        }
    }
}
