using UnityEngine;

public class WeaponPickUp : MonoBehaviour
{
    

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerController>(out var player))
        {
            Debug.Log("The player is here!");
        }
    }
}
