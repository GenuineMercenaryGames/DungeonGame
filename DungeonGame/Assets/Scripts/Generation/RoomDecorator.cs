using UnityEngine;

public class RoomDecorator : MonoBehaviour
{
    [Tooltip("Possible prefabs to spawn")]
    public GameObject[] decorationPrefabs;

    [Tooltip("Planes to decorate")]
    public GameObject[] roomPlanes;

    [Tooltip("Number of objects per 100 square units")]
    public float objectDensity = 5f;

    public void DecoratePlanes()
    {
        foreach (GameObject plane in roomPlanes)
        {
            Vector3 planePos = plane.transform.position;
            Vector3 planeScale = plane.transform.localScale;

            float planeWidth = 10f * planeScale.x;
            float planeLength = 10f * planeScale.z;

            float planeArea = planeWidth * planeLength;
            int objectsToSpawn = Mathf.RoundToInt(planeArea * objectDensity / 100f);

            for (int i = 0; i < objectsToSpawn; i++)
            {
                float randomX = Random.Range(-planeWidth / 2f, planeWidth / 2f);
                float randomZ = Random.Range(-planeLength / 2f, planeLength / 2f);

                Vector3 spawnPos =
                    planePos +
                    plane.transform.right * randomX +
                    plane.transform.forward * randomZ;

                GameObject prefab = decorationPrefabs[Random.Range(0, decorationPrefabs.Length)];

                Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                Instantiate(prefab, spawnPos, rot);
            }
            Destroy(plane);
        }
    }

    void Start()
    {
        DecoratePlanes();
    }
}