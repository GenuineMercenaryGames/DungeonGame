using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform bulletSpawnTransform;

    [Header("Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float timeBetweenShots;

    // TODO : Use an actual weapon scriptable object here for the weapon settings.

    private float elapsedTime;

    private ObjectPoolController pool;

    void Awake()
    {
        elapsedTime = 0.0f;
    }

    void Start()
    {
        pool = ObjectPoolManager.Instance.GetObjectPool(bulletPrefab);
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
        bullet.Init();

        // Notify success
        return true;
    }
}
