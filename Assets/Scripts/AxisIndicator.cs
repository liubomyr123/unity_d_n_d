using System.Collections.Generic;
using UnityEngine;

struct AxisData
{
    public Vector3 Direction;
    public string Name;
    public Color Color;
    public Quaternion Rotation;

    public AxisData(Vector3 direction, string name, Color color, Quaternion rotation)
    {
        Direction = direction;
        Name = name;
        Color = color;
        Rotation = rotation;
    }
}

public class AxisIndicator : MonoBehaviour
{
    private Transform axisRoot;
    private readonly float axisLength = 0.15f;

    private readonly List<AxisData> axesInfo = new()
    {
        new AxisData(Vector3.right,  "X_Pos", Color.red,   Quaternion.Euler(0, 0, -90)),
        new AxisData(Vector3.left,   "X_Neg", Color.red,   Quaternion.Euler(0, 0, 90)),
        new AxisData(Vector3.up,     "Y_Pos", Color.green, Quaternion.identity),
        new AxisData(Vector3.down,   "Y_Neg", Color.green, Quaternion.Euler(180, 0, 0)),
        new AxisData(Vector3.forward,"Z_Pos", Color.blue,  Quaternion.Euler(90, 0, 0)),
        new AxisData(Vector3.back,   "Z_Neg", Color.blue,  Quaternion.Euler(-90, 0, 0))
    };

    private readonly List<(Vector3 direction, LineRenderer renderer)> axisLines = new();

    void Start()
    {
        CreateAxisRootAndLines();
    }

    private void CreateAxisRootAndLines()
    {
        axisRoot = new GameObject("AxisRoot").transform;
        axisRoot.SetParent(Camera.main.transform);
        axisRoot.localScale = Vector3.one;

        foreach (AxisData axis in axesInfo)
        {
            LineRenderer line = CreateAxisLine($"Axis_{axis.Name}", axisRoot, axis.Color);
            axisLines.Add((axis.Direction, line));
            CreateAxisArrow(axis.Direction, axis.Color, axis.Rotation, $"Arrow_{axis.Name}");
        }

        CreateCenterCube();
        UpdateAxisLinesPositions();
    }

    private void LateUpdate()
    {
        if (Camera.main == null || axisRoot == null)
        {
            return;
        }

        // AxisIndicator postion in rigth top of Viewport
        Vector3 viewportPos = new Vector3(0.9f, 0.85f, 2f);
        axisRoot.position = Camera.main.ViewportToWorldPoint(viewportPos);
        axisRoot.rotation = Quaternion.identity;

        UpdateAxisLinesPositions();
    }

    private void UpdateAxisLinesPositions()
    {
        foreach (var (dir, renderer) in axisLines)
        {
            renderer.SetPosition(0, Vector3.zero);
            renderer.SetPosition(1, dir.normalized * axisLength);
        }
    }

    private void CreateAxisArrow(Vector3 direction, Color color, Quaternion rotation, string name)
    {
        GameObject axisArrow = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        axisArrow.name = name;
        axisArrow.transform.SetParent(axisRoot);
        axisArrow.transform.localPosition = direction * 0.15f;
        axisArrow.transform.localRotation = rotation;
        axisArrow.transform.localScale = new Vector3(0.03f, 0.01f, 0.03f);
        Destroy(axisArrow.GetComponent<Collider>());

        if (axisArrow.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material = CreateTransparentMaterial(
                new Color(color.r, color.g, color.b, 0.9f)
            );
        }
    }

    private void CreateCenterCube()
    {
        GameObject centerCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        centerCube.transform.SetParent(axisRoot);
        centerCube.transform.localPosition = Vector3.zero;
        centerCube.transform.localRotation = Quaternion.identity;
        centerCube.transform.localScale = Vector3.one * 0.06f;
        Destroy(centerCube.GetComponent<Collider>());

        if (centerCube.TryGetComponent<Renderer>(out var renderer))
        {
            renderer.material = CreateTransparentMaterial(
                new Color(0.5f, 0.5f, 0.5f, 0.9f)
            );
        }
    }

    private LineRenderer CreateAxisLine(string name, Transform parent, Color color)
    {
        GameObject axisLineGameObject = new GameObject(name);
        axisLineGameObject.transform.SetParent(parent);
        axisLineGameObject.transform.localPosition = Vector3.zero;
        axisLineGameObject.transform.localRotation = Quaternion.identity;
        axisLineGameObject.transform.localScale = Vector3.one;

        LineRenderer axisLineLineRenderer = axisLineGameObject.AddComponent<LineRenderer>();
        axisLineLineRenderer.positionCount = 2;
        axisLineLineRenderer.useWorldSpace = false;
        axisLineLineRenderer.startWidth = 0.01f;
        axisLineLineRenderer.endWidth = 0.01f;

        Material axisLineMaterial = new Material(Shader.Find("Sprites/Default"));
        axisLineMaterial.color = new Color(color.r, color.g, color.b, 0.75f);
        axisLineLineRenderer.material = axisLineMaterial;
        axisLineLineRenderer.sortingOrder = 1000;

        return axisLineLineRenderer;
    }

    private Material CreateTransparentMaterial(Color color)
    {
        Material newMaterial = new Material(Shader.Find("Sprites/Default"));
        newMaterial.color = color;
        newMaterial.SetFloat("_Mode", 3);
        newMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        newMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        newMaterial.SetInt("_ZWrite", 0);
        newMaterial.DisableKeyword("_ALPHATEST_ON");
        newMaterial.EnableKeyword("_ALPHABLEND_ON");
        newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        newMaterial.renderQueue = 3000;
        return newMaterial;
    }
}
