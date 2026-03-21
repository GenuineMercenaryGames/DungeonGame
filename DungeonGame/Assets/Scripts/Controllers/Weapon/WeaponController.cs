using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform bulletSpawnTransform;

    [Header("Settings")]
    [SerializeField] private WeaponDefinition weaponDefinition;

    private float elapsedTime;

    private static ObjectPoolController pool;
    private static bool poolGotten = false;

    void Awake()
    {
        elapsedTime = 0.0f;
    }

    void Start()
    {
        if (!poolGotten)
        {
            pool = ObjectPoolManager.Instance.GetObjectPool(bulletPrefab);
        }
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public bool Attack()
    {
        // ROF check
        if (elapsedTime < timeBetweenShots)
            return false;
        elapsedTime = 0.0f;

        // Spawn the bullet
        // var obj = Instantiate(bulletPrefab);
        var bullet = pool.Get<BulletController>();
        bullet.transform.position = bulletSpawnTransform.position;
        bullet.transform.rotation = bulletSpawnTransform.rotation;
        bullet.Owner = gameObject;
        bullet.Init(); // Re-init con la llamada de Init desde Awake()?

        // Notify success
        return true;
    }
}
