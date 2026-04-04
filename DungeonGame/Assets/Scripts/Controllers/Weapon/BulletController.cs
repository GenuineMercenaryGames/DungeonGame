using UnityEngine;

public class BulletController : PooledObject
{
    #region Variables

    private Rigidbody rb;

    [Header("Bullet Settings")]
    [SerializeField] public float Damage;
    [SerializeField] public float Speed;
    [SerializeField] public float LifeTime;
    [SerializeField] public int Bounces;

    public GameObject Owner;
    private GameObject lastCollidedObject;
    private float elapsedTime;
    private int bounces; // total impacts that have taken place

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Init(null);
    }

    public void Init(WeaponUser weaponUser)
    {
        lastCollidedObject = null;
        elapsedTime = 0.0f;
        bounces = 0;
        rb.linearVelocity = transform.forward * Speed;
        rb.angularVelocity = Vector3.zero;

        if (weaponUser != null)
        {
            Damage = weaponUser.weaponController.BaseDamage;
            Owner = weaponUser.gameObject;
        }
    }

    void Update()
    {
        if (elapsedTime >= LifeTime)
        {
            DestroyBullet();
        }

        elapsedTime += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Owner)
            DestroyBullet();

        if (collision.gameObject.TryGetComponent<HealthController>(out var health))
        {
            bool canDamage = true;
            if (collision.gameObject.TryGetComponent<EntityTeamController>(out var teamOther) && Owner.TryGetComponent<EntityTeamController>(out var teamOwner))
                if(teamOther.Team == teamOwner.Team)
                    canDamage = false;

            if(canDamage)
                health.Health.Value -= Damage;
            
            DestroyBullet();
        }

        if (lastCollidedObject == collision.gameObject)
        {
            return;
        }

        lastCollidedObject = collision.gameObject;

        Vector3 L = transform.forward;
        Vector3 N = collision.GetContact(0).normal;
        Vector3 R = Vector3.Reflect(L, N);
        transform.rotation = Quaternion.LookRotation(R, Vector3.up);
        rb.linearVelocity = transform.forward * Speed;

        if (bounces >= Bounces)
        {
            DestroyBullet();
        }

        ++bounces;
    }

    #endregion

    #region PrivateMethods

    private void DestroyBullet()
    {
        // The old implementation just destroys the bullet's gameobject and that's it. The new implementation makes use of object pooling if possible.
        // If the bullet was spawned from a pool, it will be automatically returned. Otherwise, it will be destroyed.
        // this.gameObject.SetActive(false);
        // Destroy(this.gameObject);

        Return();
    }

    #endregion
}
