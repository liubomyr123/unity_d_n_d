using UnityEngine;

public class PinSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pinPrefab;
    [SerializeField] private BoxCollider groundCollider;
    [SerializeField] private int pinAmount = 2;
    void Start()
    {
        for (int i = 0; i < pinAmount; i++)
        {
            Vector3 startPos = RandomPointInBounds(groundCollider.bounds);
            GameObject box = Instantiate(pinPrefab, startPos, Quaternion.identity);
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
