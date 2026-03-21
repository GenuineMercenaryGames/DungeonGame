using UnityEngine;

[RequireComponent(typeof(SingleProjectileWeapon))]
public class ShotgunProjectileWeapon : MonoBehaviour
{
    [SerializeField] private int pelletCount;

    private SingleProjectileWeapon sp;

    void Start()
    {
        sp = GetComponent<SingleProjectileWeapon>();
    }

    public void Shoot()
    {
        sp.Shoot(pelletCount);
    }
}
