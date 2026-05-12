using UnityEngine;

public class HealingPickUp : MonoBehaviour
{
    [SerializeField] public float HealingAmount;

    void OnTriggerEnter(Collider other)
    {
        // NOTE : Only heals the player. It could be made to heal any entity, but we want to avoid random entities from picking up the first aid kits.
        if (other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            // Only heal the player if they can pick up any more health, so as to prevent FAKs from being consumed unnecessarily by mistake when walking over them.
            if (player.healthController.Health.Value < player.healthController.MaxHealth.Value)
            {
                player.healthController.Health.Value += HealingAmount;
                Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }
    }
}
