using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    public GameObject treePrefab;
    public GameObject[] treeSpawnAreas;
    public NavMeshController navMeshController;

    [Tooltip("Number of trees per 100 square units of plane")]
    public float treeDensity = 5f;

    public void SpawnTrees()
    {
        foreach (GameObject plane in treeSpawnAreas)
        {
            Vector3 planePos = plane.transform.position;
            Vector3 planeScale = plane.transform.localScale;

            float planeWidth = 10f * planeScale.x;
            float planeLength = 10f * planeScale.z;

            float planeArea = planeWidth * planeLength;
            int treesToSpawn = Mathf.RoundToInt(planeArea * treeDensity / 100f);

            for (int i = 0; i < treesToSpawn; i++)
            {
                float randomX = Random.Range(-planeWidth / 2f, planeWidth / 2f);
                float randomZ = Random.Range(-planeLength / 2f, planeLength / 2f);

                Vector3 spawnPos =
                    planePos +
                    plane.transform.right * randomX +
                    plane.transform.forward * randomZ;

                Quaternion rot = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                Instantiate(treePrefab, spawnPos, rot);
            }

            plane.GetComponent<MeshRenderer>().enabled = false;
        }
    }

    void Start()
    {
        SpawnTrees();
    }
}