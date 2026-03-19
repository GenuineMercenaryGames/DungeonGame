using UnityEngine;

// TODO : Maybe consider implementing a limited amount of energy / healing capacity per healing station? kinda like Half Life.
// Also maybe consider making shield/energy/protection stations, idk if that's a good idea or not tho.
public class HealingStationController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Animator animator;

    [Header("Healing Settings")]
    [SerializeField] private float healingAmount;

    private int numUsers;

    void Awake()
    {
        numUsers = 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthController>(out var health))
        {
            if (numUsers <= 0)
                animator.SetTrigger("TriggerHealingStart");
            ++numUsers;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<HealthController>(out var health))
        {
            --numUsers;
            if (numUsers <= 0)
                animator.SetTrigger("TriggerHealingStop");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<HealthController>(out var health))
        {
            health.Health.Value += healingAmount * Time.deltaTime;
        }
    }
}
