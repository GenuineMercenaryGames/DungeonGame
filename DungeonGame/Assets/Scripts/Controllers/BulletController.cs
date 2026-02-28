using UnityEngine;

public class BulletController : MonoBehaviour
{
    #region Variables

    private Rigidbody rb;

    [Header("Bullet Settings")]
    [SerializeField] public float Damage;
    [SerializeField] public float Speed;
    [SerializeField] public float LifeTime;

    private float elapsedTime;

    #endregion

    #region MonoBehaviour

    void Awake()
    {
        elapsedTime = 0.0f;
        rb = GetComponent<Rigidbody>();
    }

    void OnEnable()
    {
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
        // TODO : Implement other bullet impact logic. For now, just destroy the gameobject.
        DestroyBullet();
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
