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
        if (lastCollidedObject == collision.gameObject)
        {
            return;
        }

        // TODO : Implement other bullet impact logic. For now, just destroy the gameobject.
        // TODO : Check if the collided with target contains a health component. If so, ignore bounce logic and just kill the projectile because it already impacted with an imaginarily squishy killable thing.

        Debug.Log($"collision {bounces}");

        lastCollidedObject = collision.gameObject;

        Vector3 L = transform.forward;
        Vector3 N = collision.GetContact(0).normal;
        Vector3 R = Vector3.Reflect(L, N);
        // transform.forward = R;
        rb.linearVelocity = R * Speed;

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
        // TODO : Implement object pooling logic later on. For now, we just destroy the bullet's gameobject and call it a day.
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    #endregion
}
