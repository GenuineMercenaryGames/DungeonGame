using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/Weapons/Attacks/Ranged Attack")]
public class RangedAttackDefinition : WeaponAttackDefinition
{
    [Header("Ranged Base")]
    [SerializeField] private float baseRange = 8f;
    [SerializeField] private int baseProjectileCount = 1;
    [SerializeField] private float baseSpreadDegrees = 0f;

    private static ObjectPoolController pool;

    //ObjectPoolController bulletPool;
    //[SerializeField] private GameObject defaultProjectilePrefab;

    private float elapsedTime;

    void Awake()
    {
        elapsedTime = 0.0f;
        //pool = ObjectPoolManager.Instance.GetObjectPool(bulletPrefab);
    }

    public override void BuildBaseContext(in WeaponDefinitionBase weapon, ref AttackContext ctx)
    {
        BasicGunDefinition gunDef = (BasicGunDefinition)weapon;

        ctx = default;

        ctx.attacKind = AttackKind; // also could be p.WeaponDef.AttackDefinition.AttackKind

        // Base stats
        ctx.damage = weapon.BaseDamage;
        ctx.attacksPerSecondMult = 1f;
        ctx.range = baseRange;

        // Ranged data
        ctx.projectileCount = baseProjectileCount;
        ctx.spreadDegrees = baseSpreadDegrees;
        //ctx.projectilePrefab = defaultProjectilePrefab;
        ctx.projectilePrefab = gunDef.bulletPrefab;
    }

    public override bool Execute(Transform weaponTransform, Vector3 aimDir, in AttackContext ctx)
    {
        Debug.Log(
            $"[Ranged Execute] dmg={ctx.damage}, ignite={ctx.igniteOnHit}, projCount={ctx.projectileCount}, spread={ctx.spreadDegrees}\n" +
            $"Origin={weaponTransform}, Direction={aimDir}"
        );

        // Spawn hitbox, projectiles, etc.
        if (elapsedTime < ctx.timeBetweenAttacks)
            return false;
        elapsedTime = 0.0f;

        // Spawn the bullet
        // var obj = Instantiate(bulletPrefab);
        //var bullet = ctx.projectilePrefab;
        //bullet.transform.position = weaponTransform.position;
        //bullet.transform.rotation = weaponTransform.rotation;
        //
        //// Instantiate
        ////bullet.Init();
        //ObjectPoolManager.Instance.GetObjectPool(bullet);
        //var obj = Instantiate(bullet);

        pool = ObjectPoolManager.Instance.GetObjectPool(ctx.projectilePrefab);
        var bullet = pool.Get<BulletController>();
        bullet.transform.position = weaponTransform.position;
        bullet.transform.rotation = weaponTransform.rotation;
        bullet.Owner = ctx.attacker;
        bullet.Init(); // Re-init con la llamada de Init desde Awake()?

        return true;
    }
}