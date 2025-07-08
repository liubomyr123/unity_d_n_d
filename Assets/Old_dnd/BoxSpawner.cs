using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private GameObject boxPrefab;
    [SerializeField] private BoxCollider groundCollider;
    [SerializeField] private int boxAmount = 2;

    private static int globalBoxId = 0;

    void Start()
    {
        for (int i = 0; i < boxAmount; i++)
        {
            Vector3 startPos = RandomPointInBounds(groundCollider.bounds);
            GameObject box = Instantiate(boxPrefab, startPos, Quaternion.identity);

            globalBoxId++;
            BoxIdentity identity = box.AddComponent<BoxIdentity>();
            identity.boxId = globalBoxId;

            box.name = "Box_" + globalBoxId;
        }
    }

    public static Vector3 RandomPointInBounds(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            1f,
            Random.Range(bounds.min.z, bounds.max.z)
        );
    }
}
