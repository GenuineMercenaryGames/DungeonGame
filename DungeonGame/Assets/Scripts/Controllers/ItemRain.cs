using UnityEngine;

public class ItemRain : MonoBehaviour
{
    [System.Serializable]
    public struct Entry
    {
        public GameObject prefab;
        public float weight;
    }

    [SerializeField] private Transform spawnTransform;
    [SerializeField] private int minSpawns = 3;
    [SerializeField] private int maxSpawns = 10;
    [SerializeField] private Entry[] items;

    public void SpawnItemRain()
    {
        if (items.Length <= 0)
            return; // Cannot spawn if no items exist lol...

        int numItems = Random.Range(minSpawns, maxSpawns);
        for (int i = 0; i < numItems; ++i)
        {
            SpawnItem(GetRandomPrefab(), spawnTransform.position);
        }
    }

    public GameObject GetRandomPrefab()
    {
        // Ensures distribution with normalized weights...

        float totalWeight = 0.0f;
        foreach(var  item in items)
            totalWeight += Mathf.Abs(item.weight);

        float randomValue = Random.Range(0.0f, totalWeight);

        float current = 0.0f;
        foreach (var item in items)
        {
            current += Mathf.Abs(item.weight);

            if (randomValue <= current)
                return item.prefab;
        }

        return items[0].prefab; // This should never happen... but the fallback is here just in case it fails.
    }

    public void SpawnItem(GameObject prefab, Vector3 spawnPosition)
    {
        float ex = Random.Range(0.0f, 0.1f);
        float ey = Random.Range(0.0f, 0.1f);
        float ez = Random.Range(0.0f, 0.1f);
        var pool = ObjectPoolManager.Instance.GetObjectPool(prefab);
        var obj = pool.Get();
        obj.SetActive(true);
        obj.transform.position = spawnPosition + new Vector3(ex, ey, ez);
        var rb = obj.GetComponent<Rigidbody>();

        // Add forces if the spawned item has RB. Otherwise, just leave on the ground.
        if (rb != null)
        {
            rb.angularVelocity = Vector3.zero;
            rb.linearVelocity = Vector3.zero;
            Vector2 circle = Random.insideUnitCircle.normalized;
            Vector3 dir = new Vector3(circle.x, 0.8f, circle.y).normalized;
            Vector3 f = dir * Random.Range(2.0f, 5.0f);
            rb.AddForce(f);
        }
    }
}
