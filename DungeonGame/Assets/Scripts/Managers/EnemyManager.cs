using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies = new();

    void Start()
    {
        ObservableVariable<float>.FuncIn2<float> callback = (oldHealth, newHealth) =>
        {
            if (oldHealth > 0.0f && newHealth <= 0.0f)
            {
                Debug.Log("Enemy Died");
            }
        };

        foreach (var enemy in enemies)
        {
            enemy.GetComponent<HealthController>().Health.AddListener(callback);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
