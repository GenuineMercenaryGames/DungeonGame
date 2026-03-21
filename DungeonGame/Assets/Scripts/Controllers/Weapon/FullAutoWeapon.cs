using UnityEngine;

[RequireComponent(typeof(SingleProjectileWeapon))]
public class AutomaticProjectileWeapon : MonoBehaviour
{
    private SingleProjectileWeapon sp;
    private bool isShooting;

    void Start()
    {
        sp = GetComponent<SingleProjectileWeapon>();
        isShooting = false;
    }

    void Update()
    {
        if (isShooting)
            sp.Shoot();
    }

    public void ShootStart()
    {
        isShooting = true;
    }

    public void ShootStop()
    {
        isShooting = false;
    }
}
