using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class SingleProjectileWeapon : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float timeBetweenShots;
    [SerializeField] private float spreadAngle;
    
    private WeaponController weaponController;
    private float elapsedTime;
    private ObjectPoolController pool;
    
    void Start()
    {
        pool = ObjectPoolManager.Instance.GetObjectPool(projectilePrefab);
        weaponController = GetComponent<WeaponController>();
        elapsedTime = timeBetweenShots;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public bool CanShoot()
    {
        return elapsedTime >= timeBetweenShots;
    }

    private void SpawnProjectile()
    {
        var bullet = pool.Get<BulletController>();
        bullet.transform.position = weaponController.weaponUser.bulletSpawnTransform.position;
        bullet.transform.rotation = Quaternion.LookRotation(GetRandomSpreadDirection(weaponController.weaponUser.bulletSpawnTransform.forward, spreadAngle));
        bullet.Init(weaponController.weaponUser);
    }

    public void Shoot()
    {
        if (!CanShoot())
            return;
        elapsedTime = 0.0f;
        SpawnProjectile();
        weaponController.weaponUser.ShootNotify();
    }

    public void Shoot(int count)
    {
        if (!CanShoot())
            return;
        elapsedTime = 0.0f;
        for (int i = 0; i < count; ++i)
            SpawnProjectile();
        weaponController.weaponUser.ShootNotify();
    }

    private Vector3 GetRandomSpreadDirection(Vector3 forward, float angle)
    {
        Vector2 randomCircle = Random.insideUnitCircle;
        float spreadRadius = Mathf.Tan(angle * Mathf.Deg2Rad * 0.5f);
        float spreadX = randomCircle.x * spreadRadius;
        float spreadY = randomCircle.y * spreadRadius * 0.1f; // Old: randomCircle.y * spreadRadius
        float spreadZ = 1.0f;
        Vector3 spread = new Vector3(spreadX, spreadY, spreadZ);
        spread = spread.normalized;
        // NOTE: Using 0.0 on Y spread is a temporary hack to make it so that there is no vertical spread.
        // Leads to better gunplay on a top down game.
        Quaternion rot = Quaternion.LookRotation(forward);
        Vector3 direction = rot * spread;
        return direction;
    }
}
