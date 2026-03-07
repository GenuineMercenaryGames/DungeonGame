using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Definition of a weapon. What is the weapon, basic stats, attack nature (WeaponAttackDefinition)
/// </summary>
public abstract class WeaponDefinitionBase : ItemDefinitionBase
{
    [Header("Weapon")]
    [SerializeField] private WeaponAttackDefinition attackDefinition;       // Reference to the weapon asset that defines how weapon atatcks
    [SerializeField] private GameObject weaponSkin;

    [Header("Base Weapon Stats")]
    [Min(0f)][SerializeField] private float baseDamage = 1f;                // Base damage of the weapon
    [Min(0.01f)][SerializeField] private float attacksPerSecond = 2f;       // AttackSpeed

    // Opcional: mods propios del arma (para armas especiales) además de pasivos globales.
    [Header("Weapon Modules (Optional)")]
    [SerializeField] private List<ScriptableObject> weaponModules = new();  // To apply optional modules. E.g. a katana always applies bleed, a shotgun always has spread (big cone)

    public WeaponAttackDefinition AttackDefinition
    {
        get { return attackDefinition; }
    }
    public float BaseDamage
    {
        get { return baseDamage; }
    }
    public float AttacksPerSecond
    {
        get { return attacksPerSecond; }
    }
    public IReadOnlyList<ScriptableObject> WeaponModules
    {
        get { return weaponModules; }
    }
    
    /// <summary>
    /// Debug funtion to know the state of the wepon definition.
    /// </summary>
    /// <returns> Validity of the definition </returns>
    public virtual bool IsValid()
    {
        return attackDefinition != null && baseDamage >= 0f && attacksPerSecond > 0f;
    }
}