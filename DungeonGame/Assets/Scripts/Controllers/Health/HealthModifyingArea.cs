using UnityEngine;

public class HealthModifyingArea : MonoBehaviour
{
    [SerializeField] private float amount;

    void OnTriggerStay(Collider collider)
    {
        if (collider.TryGetComponent<HealthController>(out var health))
        {
            health.Health.Value += amount * Time.deltaTime;
        }
    }
}
