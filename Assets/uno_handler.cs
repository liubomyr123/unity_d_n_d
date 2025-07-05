using System.Collections.Generic;
using UnityEngine;

public class uno_handler : MonoBehaviour
{
    private List<Transform> pins = new List<Transform>();
    
    private Dictionary<Transform, Color> originalColors =
            new Dictionary<Transform, Color>();
    private Dictionary<Transform, MeshRenderer> pinVisualRenderers = 
            new Dictionary<Transform, MeshRenderer>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        List<string> names = new List<string> { "A0", "A1", "A2", "A3", "A4", "A5",
            "Bottom_GND",
            "Bottom_RESET",
            "Bottom_PB5",
            "Bottom_OUT_5V",
            "Bottom_PB4",
        };

        foreach (var item in names)
        {
            Transform pin = FindChildByName(this.gameObject.transform, item);
            if (pin != null)
            {
                Debug.Log($"Знайдено об'єкт: {pin.name}");
                pins.Add(pin);

                MeshRenderer rend = pin.GetComponentInChildren<MeshRenderer>();
                if (rend == null)
                {
                    GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    visual.transform.SetParent(pin);
                    visual.transform.localPosition = Vector3.zero;
                    visual.transform.localScale = Vector3.one * 0.1f;
                    Destroy(visual.GetComponent<Collider>());

                    rend = visual.GetComponent<MeshRenderer>();

                    originalColors[pin] = rend.material.color;
                    pinVisualRenderers[pin] = rend;
                }
                else
                {
                    // Якщо MeshRenderer є у піна, теж збережемо для зручності
                    originalColors[pin] = rend.material.color;
                    pinVisualRenderers[pin] = rend;
                }
            }
            else
            {
                Debug.LogWarning($"Не знайдено елемент з ім'ям: {name}");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float maxDistance = 2f;

        foreach (Transform pin in pins)
        {
            if (pin == null) continue;

            Vector3 origin = pin.position;
            Vector3 direction = Vector3.up;

            if (!pinVisualRenderers.ContainsKey(pin)) continue;


            MeshRenderer rend = pinVisualRenderers[pin];

            if (Physics.Raycast(origin, direction, out RaycastHit hit, maxDistance))
            {
                Debug.DrawLine(origin, hit.point, Color.red, 1f);
                ObjectIdentifier idComp = hit.collider.GetComponent<ObjectIdentifier>();

                if (idComp != null)
                {
                    Debug.Log($"⬆️ Над {pin.name} виявлено об’єкт: {idComp.Type}/{idComp.Id}");
                }
                else
                {
                    Debug.Log($"⬆️ Над {pin.name} виявлено об’єкт без ObjectIdentifier: {hit.collider.name}");
                }

                rend.material.color = Color.red;
            }
            else
            {
                Debug.DrawRay(origin, direction * maxDistance, Color.green, 1f);

                if (originalColors.ContainsKey(pin))
                {
                    rend.material.color = originalColors[pin];
                }
            }
        }
    }

    // Transform child = FindChildByName(myObject.transform, "child_name");
    public Transform FindChildByName(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform result = FindChildByName(child, name);
            if (result != null)
                return result;
        }
        return null;
    }
}
