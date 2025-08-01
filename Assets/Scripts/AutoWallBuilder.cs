using UnityEngine;

[ExecuteInEditMode] // optional: allows building in editor
public class AutoWallBuilder : MonoBehaviour
{
    public GameObject wallParent;
    public float wallThickness = 1f;
    public float wallHeight = 5f;
    public bool autoRebuild = false;

    private string[] wallNames = { "Wall_Top", "Wall_Bottom", "Wall_Left", "Wall_Right" };

    void Start()
    {
        BuildWalls();
    }

    void OnValidate()
    {
        if (autoRebuild)
        {
            ClearExistingWalls();
            BuildWalls();
            autoRebuild = false;
        }
    }

    public void BuildWalls()
    {
        if (wallParent == null)
        {
            wallParent = new GameObject("WallBounds");
            wallParent.transform.parent = transform;
        }

        ClearExistingWalls();

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null)
        {
            Debug.LogWarning("AutoWallBuilder: No Renderer found on this GameObject.");
            return;
        }

        Vector3 size = renderer.bounds.size;
        Vector3 center = renderer.bounds.center;

        // Walls positions relative to the plane
        Vector3[] positions = new Vector3[]
        {
            new Vector3(center.x, center.y + wallHeight / 2, center.z + size.z / 2 + wallThickness / 2), // Top
            new Vector3(center.x, center.y + wallHeight / 2, center.z - size.z / 2 - wallThickness / 2), // Bottom
            new Vector3(center.x - size.x / 2 - wallThickness / 2, center.y + wallHeight / 2, center.z), // Left
            new Vector3(center.x + size.x / 2 + wallThickness / 2, center.y + wallHeight / 2, center.z)  // Right
        };

        Vector3[] scales = new Vector3[]
        {
            new Vector3(size.x + wallThickness * 2, wallHeight, wallThickness), // Top
            new Vector3(size.x + wallThickness * 2, wallHeight, wallThickness), // Bottom
            new Vector3(wallThickness, wallHeight, size.z + wallThickness * 2), // Left
            new Vector3(wallThickness, wallHeight, size.z + wallThickness * 2), // Right
        };

        for (int i = 0; i < 4; i++)
        {
            GameObject wall = new GameObject(wallNames[i]);
            wall.transform.position = positions[i];
            wall.transform.localScale = scales[i];
            wall.transform.parent = wallParent.transform;

            BoxCollider col = wall.AddComponent<BoxCollider>();
            col.isTrigger = false;

            // OPTIONAL: make visible temporarily for debug
            // Uncomment this line to see the wall visually:
            // wall.AddComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    void ClearExistingWalls()
    {
        if (wallParent != null)
        {
            DestroyImmediate(wallParent);
        }
    }
}