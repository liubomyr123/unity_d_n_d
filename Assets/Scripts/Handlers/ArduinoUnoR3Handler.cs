using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

public class ArduinoUnoR3Handler : MonoBehaviour
{
    private readonly List<Transform> headerPins = new List<Transform>();
    
    private readonly Dictionary<Transform, Color> originalColors =
            new Dictionary<Transform, Color>();
    private readonly Dictionary<Transform, MeshRenderer> headerPinVisualRenderers = 
            new Dictionary<Transform, MeshRenderer>();

    void Start()
    {
        InitHeaderPins();
    }

    private void InitHeaderPins()
    {
        List<HeaderPinIdentifier> headerPinsList = ArduinoUnoR3Pins.GetHeaderPins();
        foreach (var headerPin in headerPinsList)
        {
            Transform headerPinChild = FindChildByName(this.gameObject.transform, headerPin.Name);
            if (headerPinChild == null)
            {
                Debug.LogError($"Header pin [{headerPin.Name}] was not found");
                continue;
            }

            // Store the pin's transform
            headerPins.Add(headerPinChild);

            // Initialize the headerPin identifier
            BoardObjectIdentifier headerPinIdentifier = headerPinChild.GetComponent<BoardObjectIdentifier>();
            if (headerPinIdentifier == null)
            {
                headerPinIdentifier = headerPinChild.gameObject.AddComponent<BoardObjectIdentifier>();
            }
            string uniqueId = System.Guid.NewGuid().ToString();
            headerPinIdentifier.SetId(uniqueId, headerPin.Type.ToString());

            // Try to get the MeshRenderer for the pin
            MeshRenderer headerPinRenderer = headerPinChild.GetComponentInChildren<MeshRenderer>();
            if (headerPinRenderer == null)
            {
                // If no visual exists, create a small sphere as the pin's visual
                GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                visual.transform.SetParent(headerPinChild);
                visual.transform.localPosition = Vector3.zero;
                visual.transform.localScale = Vector3.one * 0.1f;

                // Remove the collider so it doesn't interfere with physics (e.g., raycasting)
                Destroy(visual.GetComponent<Collider>());

                // Get the MeshRenderer from the created visual
                headerPinRenderer = visual.GetComponent<MeshRenderer>();

                // Store the original color and the renderer reference
                originalColors[headerPinChild] = headerPinRenderer.material.color;
                headerPinVisualRenderers[headerPinChild] = headerPinRenderer;
            }
            else
            {
                // If visual already exists, just store the color and renderer
                originalColors[headerPinChild] = headerPinRenderer.material.color;
                headerPinVisualRenderers[headerPinChild] = headerPinRenderer;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHeaderPins();
    }

    void UpdateHeaderPins()
    {
        const float maxDistance = 2f;

        foreach (var handler in FemaleDupontHandler.All)
        {
            handler.hoveringHeaderPin = null;
        }
        foreach (var handler in MaleDupontHandler.All)
        {
            handler.hoveringHeaderPin = null;
        }

        foreach (Transform headerPin in headerPins)
        {
            if (headerPin == null)
            {
                continue;
            }
            if (!headerPinVisualRenderers.ContainsKey(headerPin))
            {
                continue;
            }

            Vector3 origin = headerPin.position;
            Vector3 direction = Vector3.up;

            MeshRenderer headerPinRenderer = headerPinVisualRenderers[headerPin];

            bool isDupontHitHeader = Physics.Raycast(origin, direction, out RaycastHit anyObjectHit, maxDistance);
            if (isDupontHitHeader)
            {
                // Identifier of any object that collide with header pin
                DragUIElementIdentifier anyObjectHitIdentifier = anyObjectHit.collider.GetComponent<DragUIElementIdentifier>();
                if (anyObjectHitIdentifier != null)
                {
                    //Debug.Log($"[{anyObjectHitIdentifier.Type}] was found above the header pin [{headerPin.name}]");

                    // Identifier of header pin on board
                    BoardObjectIdentifier headerPinIdentifier = headerPin.GetComponent<BoardObjectIdentifier>();
                    if (headerPinIdentifier != null)
                    {
                        bool isFemaleHeaderPin = headerPinIdentifier.Type == HeaderPinType.FemaleHeaderPin.ToString();
                        bool isMaleHeaderPin = headerPinIdentifier.Type == HeaderPinType.MaleHeaderPin.ToString();

                        bool isFemaleDupont = anyObjectHitIdentifier.Type == "Female_Dupont";
                        bool isMaleDupont = anyObjectHitIdentifier.Type == "Male_Dupont";

                            if (isFemaleHeaderPin)
                            {
                                if (isMaleDupont)
                                {
                                    //Debug.Log($"Male dupont was found above the female header pin [{headerPin.name}]");
                                    MaleDupontHandler maleDupontHandler = anyObjectHit.collider.GetComponent<MaleDupontHandler>();
                                    bool is_attached = maleDupontHandler.IsAttached();
                                    maleDupontHandler.hoveringHeaderPin = (maleDupontHandler != null) ? headerPin : null;

                                    if (!is_attached)
                                    {
                                        headerPinRenderer.material.color = Color.green;
                                    }
                                    else
                                    {
                                        if (originalColors.ContainsKey(headerPin))
                                        {
                                            headerPinRenderer.material.color = originalColors[headerPin];
                                        }
                                    }
                                }
                                else
                                {
                                    headerPinRenderer.material.color = Color.red;
                                }
                            }
                            if (isMaleHeaderPin)
                            {
                                if (isFemaleDupont)
                                {
                                    //Debug.Log($"Female dupont was found above the male header pin [{headerPin.name}]");
                                    FemaleDupontHandler femaleDupontHandler = anyObjectHit.collider.GetComponent<FemaleDupontHandler>();
                                    bool is_attached = femaleDupontHandler.IsAttached();
                                    femaleDupontHandler.hoveringHeaderPin = (femaleDupontHandler != null) ? headerPin : null;

                                    if (!is_attached)
                                    {
                                        headerPinRenderer.material.color = Color.green;
                                    }
                                    else
                                    {
                                        if (originalColors.ContainsKey(headerPin))
                                        {
                                            headerPinRenderer.material.color = originalColors[headerPin];
                                        }
                                    }
                                }
                                else
                                {
                                    headerPinRenderer.material.color = Color.red;
                                }
                            }
                    }
                }
            }
            else
            {
                Debug.DrawRay(origin, direction * maxDistance, Color.green, 1f);
                if (originalColors.ContainsKey(headerPin))
                {
                    headerPinRenderer.material.color = originalColors[headerPin];
                }
            }
        }
    }

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
