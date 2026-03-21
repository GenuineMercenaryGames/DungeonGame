using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SetIsSelected(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            SetIsSelected(false);
        }
    }

    private void SetIsSelected(bool isSelected)
    {
        foreach (var mat in meshRenderer.materials)
        {
            mat.SetInt("_IsSelected", isSelected ?  1 : 0);
        }
    }
}
