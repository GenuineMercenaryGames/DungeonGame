using UnityEngine;

[RequireComponent(typeof(WeaponController))]
public class MeleeWeapon : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float attackRadius;
    [SerializeField] private float timeBetweenAttacks; // Melee attack cooldown of sorts.

    private WeaponController wc;
    private Collider[] results;

    private float elapsedTime;

    void Start()
    {
        wc = GetComponent<WeaponController>();
        results = new Collider[10];
        elapsedTime = timeBetweenAttacks;
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
    }

    public void Attack()
    {
        if (elapsedTime < timeBetweenAttacks)
            return;
        elapsedTime = 0.0f;
        int count = Physics.OverlapSphereNonAlloc(wc.weaponUser.bulletSpawnTransform.position, attackRadius, results);
        for (int i = 0; i < count; ++i)
        {
            if (results[i] != wc.weaponUser && results[i].TryGetComponent<HealthController>(out var health))
            {
                health.Health.Value -= damage;
            }
        }
    }
}
