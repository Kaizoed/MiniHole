using UnityEngine;
using System.Collections;

public class HoleSwallow : MonoBehaviour
{
    public float growAmount = 0.2f;
    public float maxSize = 10f;

    private Transform hole;
    private SphereCollider swallowZone;

    void Start()
    {
        hole = transform.parent;
        swallowZone = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickup"))
        {
            // Start falling animation
            StartCoroutine(SinkObject(other.gameObject));

            // Grow the hole
            if (hole.localScale.x < maxSize)
            {
                hole.localScale += new Vector3(growAmount, 0f, growAmount);
                swallowZone.radius += growAmount * 0.5f;
            }
        }
    }

    IEnumerator SinkObject(GameObject obj)
    {
        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
        }

        float duration = 1f;
        float elapsed = 0f;
        Vector3 startPos = obj.transform.position;
        Vector3 endPos = startPos + Vector3.down * 2f;

        while (elapsed < duration)
        {
            obj.transform.position = Vector3.Lerp(startPos, endPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        Destroy(obj);
    }
}