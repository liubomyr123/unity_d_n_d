using System.Collections;
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
        //RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 spawnPoint;

        //if (Physics.Raycast(ray, out hit, 1000.0f))
        //{
        //    // Якщо ми влучили в щось — ставимо туди
        //    spawnPoint = hit.point;
        //}
        //else
        //{
        //    // Інакше — просто на відстані 5 одиниць перед камерою
        //    //spawnPoint = ray.origin + ray.direction * 5.0f;
        //    // OR
        //}

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
        GameObject obj = Instantiate(
            PrefabToInstatiate,
            pos,
            Quaternion.identity);

        // Додаємо Rigidbody, якщо ще немає
        if (!obj.TryGetComponent<Rigidbody>(out _))
        {
            Rigidbody rb = obj.AddComponent<Rigidbody>();

            // За бажанням: налаштовуємо Rigidbody
            rb.useGravity = false;
            rb.isKinematic = true;
        }

        if (!obj.TryGetComponent<DragnDrop1>(out _))
        {
            FitBoxColliderToChildren(obj);
            obj.AddComponent<DragnDrop1>();
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


}
