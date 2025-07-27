using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    public GameObject objectPrefab;
    public int numberToSpawn = 10;
    public GameObject groundPlane; // assign your plane here in the Inspector

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        if (groundPlane == null) return;

        Renderer groundRenderer = groundPlane.GetComponent<Renderer>();
        if (groundRenderer == null) return;

        Vector3 planeSize = groundRenderer.bounds.size;
        Vector3 planeCenter = groundRenderer.bounds.center;

        for (int i = 0; i < numberToSpawn; i++)
        {
            float x = Random.Range(planeCenter.x - planeSize.x / 2, planeCenter.x + planeSize.x / 2);
            float z = Random.Range(planeCenter.z - planeSize.z / 2, planeCenter.z + planeSize.z / 2);

            Vector3 spawnPosition = new Vector3(x, 0.5f, z);
            Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
        }
    }
}