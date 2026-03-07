using UnityEngine;
using static UnityEngine.InputManagerEntry;

[CreateAssetMenu(menuName = "Roguelike/Weapons/Attacks/Melee Attack")]
public class MeleeAttackDefinition : WeaponAttackDefinition
{
    [SerializeField] private float baseRange = 2f;
    [SerializeField] private float arcDegrees = 90f;

    public override void BuildBaseContext(in WeaponDefinitionBase weapon, ref AttackContext ctx)
    {
        ctx = default;

        ctx.attacKind = AttackKind;

        // Base stats
        ctx.damage = weapon.BaseDamage;
        ctx.attacksPerSecondMult = 1f;
        ctx.range = baseRange;

        // Melee data
        ctx.meleeArcDegrees = arcDegrees;
        ctx.meleeRadius = baseRange;
    }

    public override bool Execute(Transform weaponTransform, Vector3 aimDir, in AttackContext ctx)
    {
        Debug.Log($"[Melee Execute] dmg={ctx.damage}, ignite={ctx.igniteOnHit}, arc={ctx.meleeArcDegrees}.\n" +
            $"Origin={weaponTransform}, Direction={aimDir}");

        // Spawn hitbox, projectiles, etc.

        return true;
    }
}