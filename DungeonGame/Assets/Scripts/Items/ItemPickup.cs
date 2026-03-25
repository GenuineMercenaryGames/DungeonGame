using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    [Header("Assign one depending on type")]
    [SerializeField] private ItemDefinitionBase m_item;

    private GameObject itemMesh;
    [SerializeField] GameObject defaultMesh;

    private void Reset()
    {
        // Para que funcione como trigger automáticamente
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Start()
    {
        RenderMesh();
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

        var weaponItem = m_item as WeaponDefinitionBase;

        if(weaponItem)
        {
            playerItems.EquipWeapon(weaponItem);
            Debug.Log($"[Pickup] Weapon collected: {weaponItem.DisplayName}");
            Destroy(gameObject);
        }
        else
        {
            var passiveItem = m_item as PassiveItemDefinitionBase;

            if(passiveItem)
            {
                playerItems.AddPasiveItem(passiveItem); // si renombras, cambia aquí
                Debug.Log($"[Pickup] Passive collected: {passiveItem.DisplayName}");
                Destroy(gameObject);
            }
            else
            {
                Debug.LogWarning("[Pickup] Misconfigured pickup (missing reference).");
            }
        }

        SfxManager.Instance.PlaySfx(AudioNames.ItemPickupSfx);
    }

    private void CreateItemPickup(ItemDefinitionBase item)
    {
        m_item = item;

        RenderMesh();
    }

    private void RenderMesh()
    {
        if(m_item == null || m_item.PickupPrefab == null)
        {
            defaultMesh.SetActive(true);
            Debug.LogWarning("[Pickup] Misconfigured pickup (missing mesh to render).");
            // itemMesh = Instantiate(defaultMesh, transform);
        }
        else
        {
            defaultMesh.SetActive(false);
            itemMesh = Instantiate(m_item.PickupPrefab, transform);
            itemMesh.transform.localPosition = Vector3.zero;
            itemMesh.transform.localRotation = Quaternion.identity;
        }
    }
}