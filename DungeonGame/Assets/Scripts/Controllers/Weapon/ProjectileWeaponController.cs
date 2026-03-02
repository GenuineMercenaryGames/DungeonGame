using UnityEngine;

public class ProjectileWeaponController : MonoBehaviour
{
    [SerializeField] private WeaponControllerTest weaponController;
    [SerializeField] private Transform spawnTransform;
    [SerializeField] private GameObject projectilePrefab;

    private float elapsedTime;
    
    void Start()
    {
        elapsedTime = weaponController.BaseTimeBetweenShots;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void Shoot()
    {
        if (elapsedTime < weaponController.BaseTimeBetweenShots)
            return;
        elapsedTime = 0.0f;

        var go = Instantiate(projectilePrefab);
        go.transform.position = spawnTransform.position;
        go.transform.rotation = spawnTransform.rotation;
        // NOTE : Here, assign the projectile damage to whatever it is plus the base weapon damage.
    }
}
