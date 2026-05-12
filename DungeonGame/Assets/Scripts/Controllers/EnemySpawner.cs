using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnTransforms;
    [SerializeField] private float timeBetweenSpawns = 2.5f;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private GameObject[] enemyPrefabs;

    private int spawnedEnemies;

    void Start()
    {
        SpawnEnemiesLoop();
    }

    private void SpawnEnemiesLoop()
    {
        spawnedEnemies = 0;
        StartCoroutine(SpawnEnemiesLoopCoroutine());
    }

    private IEnumerator SpawnEnemiesLoopCoroutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }
    }

    private void SpawnEnemy()
    {
        if (enemyPrefabs.Length <= 0) return;
        if (spawnTransforms.Length <= 0) return;
        if (spawnedEnemies >= maxEnemies) return;

        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        var go = Instantiate(prefab);
        go.transform.position = spawnTransforms[Random.Range(0, spawnTransforms.Length)].position;
        var enemy = go.GetComponent<Enemy>();
        var health = go.GetComponent<HealthController>();
        if(health == null) health = go.GetComponentInChildren<HealthController>();
        spawnedEnemies++;
        health.Health.AddListener(HandleDeath);
        enemy.playerScanningRange = 50000;
        enemy.playerFollowRange = 50000;
    }

    private void HandleDeath(float oldHealth, float newHealth)
    {
        if(newHealth <= 0 && oldHealth > 0)
            spawnedEnemies--;
    }


}
