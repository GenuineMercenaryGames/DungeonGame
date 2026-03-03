using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Acumulative pasive. Provides modules or passive effects
/// </summary>
public abstract class PassiveItemDefinitionBase : ItemDefinitionBase
{
    [Header("Passive")]
    [Min(1)]
    [SerializeField] private int maxStacks = 1; // In the future maybe we want to implement that some items can appear more than one time in a run.
                                                // It is always posible to change. Accumulative items: Isaac Ref. Bandage girl

    [SerializeField] private List<PassiveWeaponModuleDefinition> modules = new(); // Change to PassiveModuleDefinition insted of ScriptableObject

    public int MaxStacks
    {
        get { return maxStacks; }
    }

    public IReadOnlyList<PassiveWeaponModuleDefinition> Modules
    {
        get { return modules; }
    }
}