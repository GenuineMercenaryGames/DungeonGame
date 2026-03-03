using UnityEngine;

/// <summary>
/// Will add damage to the attack and add ignite tick damage.
/// </summary>
[CreateAssetMenu(menuName = "Roguelike/Modules/Fire Module")]
public class FireWeaponModule : PassiveWeaponModuleDefinition
{
    [SerializeField] private float extraDamage = 1.0f;

    public override void ModifyAttack(ref AttackContext ctx)
    {
        ctx.igniteOnHit = true;
        ctx.damage += extraDamage;

        if(ctx.attacKind == WeaponAttackDefinition.AttackKindEnum.MELEE)
        {
            Debug.Log("[FireWeaponModule] Melee weapon is in flames and ignites on hit");
        }
        else
        {
            Debug.Log("[FireWeaponModule] Ranged weapon is in flames and ignites on hit");
        }
        Debug.Log("[FireWeaponModule] Actual Damage = " + ctx.damage);
    }
}