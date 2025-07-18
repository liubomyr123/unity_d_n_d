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
        //Debug.LogWarning("OnBeginDrag");
        mOriginalPanelLocalPosition = UIDragElement.localPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            Canvas,
            data.position,
            data.pressEventCamera,
            out mOriginalLocalPointerPosition);
    }

    public void OnDrag(PointerEventData data)
    {
        //Debug.LogWarning("OnDrag");
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
        //Debug.LogWarning("OnEndDrag");
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
            // fallback, ���� �� ��������� (�����������)
            spawnPoint = ray.origin + ray.direction * 5f;
        }

        CreateObject(spawnPoint);
    }

    void CreateObject(Vector3 pos)
    {
        if (PrefabToInstatiate == null)
        {
            Debug.LogError("No prefab was found for UI element.");
            return;
        }
        string type = PrefabToInstatiate.name;

        GameObject obj = Instantiate(
            PrefabToInstatiate,
            pos,
            Quaternion.identity);

        AddCollidersToChildren(obj);

        string uniqueId = System.Guid.NewGuid().ToString();

        DragUIElementIdentifier idComp = obj.GetComponent<DragUIElementIdentifier>();
        if (idComp == null)
        {
            idComp = obj.AddComponent<DragUIElementIdentifier>();
        }
        idComp.SetId(uniqueId, type);

        switch (type)
        {
            case "Male_Dupont":
                Debug.Log("It is [Male_Dupont] element. Init MaleDupontHandler()");
                if (!obj.TryGetComponent<MaleDupontHandler>(out _))
                {
                    FitBoxColliderToChildren(obj);
                    obj.AddComponent<MaleDupontHandler>();
                }
                break;

            case "Female_Dupont":
                Debug.Log("It is [Female_Dupont] element. Init FemaleDupontHandler()");
                if (!obj.TryGetComponent<FemaleDupontHandler>(out _))
                {
                    FitBoxColliderToChildren(obj);
                    obj.AddComponent<FemaleDupontHandler>();
                }
                break;

            case "Arduino_Uno_R3":
                Debug.Log("It is [Arduino_Uno_R3] element. Init ArduinoUnoR3Handler()");
                if (!obj.TryGetComponent<ArduinoUnoR3Handler>(out _))
                {
                    FitBoxColliderToChildren(obj);
                    obj.AddComponent<ArduinoUnoR3Handler>();
                }
                break;

            default:
                Debug.LogWarning("UI element was not found");
                break;
        }

        if (!obj.TryGetComponent<Rigidbody>(out _))
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();

            rb.useGravity = false;
            rb.isKinematic = true;
        }
        if (!obj.TryGetComponent<DragnDropNew>(out _))
        {
            obj.AddComponent<DragnDropNew>();
        }
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
        // ��������� Bounds �� ������� ��'����
        Bounds bounds = new Bounds(parent.transform.position, Vector3.zero);
        bool foundAny = false;

        // ����������� �� ��� MeshRenderer-�� ����
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
            // ������ BoxCollider, ���� �����
            if (!parent.TryGetComponent<BoxCollider>(out var collider))
            {
                collider = parent.AddComponent<BoxCollider>();
            }

            // ���������� ����� bounds � �������� ����������
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
                    // ������ BoxCollider ����� ���� � Renderer
                    child.gameObject.AddComponent<BoxCollider>();
                }
            }
        }
    }
}
