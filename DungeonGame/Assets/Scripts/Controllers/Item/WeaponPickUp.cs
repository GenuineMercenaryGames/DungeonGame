using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private GameObject weaponPrefab; // NOTE : In the future, this will be the weapon SO. Don't worry, I'll get to it tomorrow when I'm back. Don't touch this yet please, It's a WIP and I'm too tired by now to deal with someone breaking this.

    public void PickUp(PlayerController player)
    {
        player.weapons[player.equipedWeaponIndex] = weaponPrefab;
        player.EquipWeapon(weaponPrefab);
        Destroy(gameObject);
    }

    public void SetIsInRadius(bool isSelected)
    {
        foreach (var mat in meshRenderer.materials)
        {
            mat.SetInt("_IsSelected", isSelected ?  1 : 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SetIsInRadius(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SetIsInRadius(false);
        }
    }
}
