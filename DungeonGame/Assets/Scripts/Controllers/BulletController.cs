using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;

    [Header("Bullet Settings")]
    [SerializeField] public float Damage;
    [SerializeField] public float Speed;
    [SerializeField] public float LifeTime;
    [SerializeField] public int Bounces;

    private GameObject lastCollidedObject;
    private float elapsedTime;
    private int bounces; // total impacts that have taken place

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
        lastCollidedObject = null;
        elapsedTime = 0.0f;
        bounces = 0;
    }

    void Start()
    {
        // NOTE : When object pooling is implemented, this bit of logic CANNOT go here. Need to move to some init function of sorts.
        rb.linearVelocity = transform.forward * Speed;
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
        if (collision.gameObject.TryGetComponent<HealthController>(out var health))
        {
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

        // TODO : Modify to make use of object pooling. Main changes should probably go in the DestroyBullet() function.
    }

    #endregion

    #region PrivateMethods

    private void DestroyBullet()
    {
        // TODO : Implement object pooling logic later on. For now, we just destroy the bullet's gameobject and call it a day.
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    #endregion
}
