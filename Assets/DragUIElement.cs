using System.Collections;
using TMPro.EditorUtilities;
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
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            Vector3 worldPoint = hit.point;
            CreateObject(worldPoint);
        }
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
}
