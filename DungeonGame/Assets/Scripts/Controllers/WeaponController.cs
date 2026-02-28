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

    void Awake()
    {
        elapsedTime = 0.0f;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void Attack()
    {
        if (elapsedTime < timeBetweenShots)
        {
            Debug.Log("CANNOT SHOOT BULLET YET");
            return;
        }
        elapsedTime = 0.0f;

        Debug.Log("SHOOTING BULLET NOW");

        // TODO : Modify the logic to make use of object pooling later on.
        var obj = Instantiate(bulletPrefab);
        obj.transform.position = bulletSpawnTransform.position;
        obj.transform.rotation = bulletSpawnTransform.rotation;
    }
}
