using System;

using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using UnityEngine.UIElements;
using static UnityEngine.InputManagerEntry;

/// <summary>
/// Necessary runtime parameters to build an attack. Photo of the moment of the attack.
/// </summary>
//[Serializable]
//public struct WeaponAttackParams
//{
//    public Transform atacker;                       // Player or enemy attacking
//    public Transform origin;                        // Origin of the attack
//    public Vector3 aimDirection;                    // Direction vector of the attack
//    public WeaponDefinitionBase weaponDefinition;   // Reference to the weapon definition
//    public float time;                              // In case in the future we need it for synchronization with anims or whatever
//}

/// <summary>
/// Mutable attack description. Final values after applying base weapon data and passives.
/// </summary>
[Serializable]
public struct AttackContext
{
    public WeaponAttackDefinition.AttackKindEnum attacKind;

    // Comunes
    public float damage;
    public float attacksPerSecondMult;
    public float range;

    // Ranged
    public int projectileCount;
    public float spreadDegrees;
    public GameObject projectilePrefab;

    // Melee (ejemplo simple)
    public float meleeArcDegrees;
    public float meleeRadius;

    // Flags simples (ej: se pueden convertir a "keywords" / tags). In future: List<OnHitEffect>
    public bool pierce;
    public bool bounce;
    public bool igniteOnHit;

    // public MeleeAttackData melee;
    //public RangedAttackData ranged;
}

// WIP
/*
[Serializable]
public struct MeleeAttackData
{
    public float arcDegrees;
    public float radius;
    public float duration;
    public float knockback;
}

[Serializable]
public struct RangedAttackData
{
    public int projectileCount;
    public float spreadDegrees;
    public GameObject projectilePrefab;
    public float projectileSpeed;
    public float projectileLifetime;
}
*/

/// <summary>
/// Controlls the attack logic, configurable by asset. It will execute base attack, without passives. How does the weapon attack, configurable by asset.
/// Spawn hitbox, projectiles, etc. once module have modified the context.
/// Is referenced by WeaponDefinitionBase.
/// </summary>
public abstract class WeaponAttackDefinition : ScriptableObject
{
    public enum AttackKindEnum
    {
        MELEE,
        RANGED
    }

    [SerializeField] private AttackKindEnum m_attackKind;   // Nature of the weapon attack

    public AttackKindEnum AttackKind
    {
        get { return m_attackKind; }
    }

    /// <summary>
    /// Build initial AttackContext
    /// Set damage, fireRate, range, knockBack, etc.
    /// Set melee/ranged specific attributes:
    ///     Ranged: projectileCount, prefab, spread...
    ///     Melee: Attack cone, radius, duration...
    /// Does not spawn anything.
    /// </summary>
    /// <param name="p"> Read-only moment of the attack. When does the attack occur. </param>
    /// <param name="ctx"> Modifiable context of the attack. What is the attack doing. </param>
    public abstract void BuildBaseContext(in WeaponDefinitionBase weapon, ref AttackContext ctx);

    /// <summary>
    /// Executes the attack once the context has been initialized.
    /// Spawn bullets (pool), hitbox, detect hits, etc.
    /// </summary>
    /// <param name="p"> Read-only moment of the attack. When does the attack occur. </param>
    /// <param name="ctx"> Read-only context, already built in BuildBaseContext. Read to execute. </param>
    public abstract void Execute(Transform attacker, Vector3 aimDir, in AttackContext ctx);
}