using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    public enum PickupType { WEAPON, PASSIVE }

    [SerializeField] private PickupType type;

    [Header("Assign one depending on type")]
    [SerializeField] private WeaponDefinitionBase weapon;
    [SerializeField] private PassiveItemDefinitionBase passiveItem;

    private void Reset()
    {
        // Para que funcione como trigger automáticamente
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        var playerItems = other.GetComponent<PlayerItemSystem>();
        if (playerItems == null)
        {
            // Si el PlayerItemSystem está en el padre del collider:
            playerItems = other.GetComponentInParent<PlayerItemSystem>();
            if (playerItems == null) return;
        }

        if (type == PickupType.WEAPON && weapon != null)
        {
            playerItems.EquipWeapon(weapon);
            Debug.Log($"[Pickup] Weapon collected: {weapon.DisplayName}");
            Destroy(gameObject);
            return;
        }

        if (type == PickupType.PASSIVE && passiveItem != null)
        {
            playerItems.AddPasiveItem(passiveItem); // si renombras, cambia aquí
            Debug.Log($"[Pickup] Passive collected: {passiveItem.DisplayName}");
            Destroy(gameObject);
            return;
        }

        Debug.LogWarning("[Pickup] Misconfigured pickup (missing reference).");
    }
}