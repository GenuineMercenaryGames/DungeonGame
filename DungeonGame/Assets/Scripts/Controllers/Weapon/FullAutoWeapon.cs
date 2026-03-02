using UnityEngine;

public class FullAutoWeapon : MonoBehaviour
{
    [SerializeField] private ProjectileWeaponController weaponController;
    
    private bool isShooting;

    void Start()
    {
        isShooting = false;
    }

    void Update()
    {
        if (isShooting)
            weaponController.Shoot();
    }

    public void ShootFullAutoBegin()
    {
        isShooting = true;
    }

    public void ShootFullAutoStop()
    {
        isShooting = false;
    }
}
