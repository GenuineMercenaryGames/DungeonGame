using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/Weapons/Attacks/Ranged Attack")]
public class RangedAttackDefinition : WeaponAttackDefinition
{
    [Header("Ranged Base")]
    [SerializeField] private float baseRange = 8f;
    [SerializeField] private int baseProjectileCount = 1;
    [SerializeField] private float baseSpreadDegrees = 0f;
    [SerializeField] private GameObject defaultProjectilePrefab;

    public override void BuildBaseContext(in WeaponDefinitionBase weapon, ref AttackContext ctx)
    {
        ctx = default;

        ctx.attacKind = AttackKind; // also could be p.WeaponDef.AttackDefinition.AttackKind

        // Base stats
        ctx.damage = weapon.BaseDamage;
        ctx.attacksPerSecondMult = 1f;
        ctx.range = baseRange;

        // Ranged data
        ctx.projectileCount = baseProjectileCount;
        ctx.spreadDegrees = baseSpreadDegrees;
        ctx.projectilePrefab = defaultProjectilePrefab;
    }

    public override void Execute(Transform attacker, Vector3 aimDir, in AttackContext ctx)
    {
        Debug.Log(
            $"[Ranged Execute] dmg={ctx.damage}, ignite={ctx.igniteOnHit}, projCount={ctx.projectileCount}, spread={ctx.spreadDegrees}\n" +
            $"Origin={attacker}, Direction={aimDir}"
        );

        // Aquí luego:
        // - spawnear ctx.projectileCount proyectiles desde p.Origin
        // - aplicar spread usando ctx.spreadDegrees
        // - setear daño/flags al proyectil según ctx
    }
}