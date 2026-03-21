using UnityEngine;

public abstract class PassiveWeaponModuleDefinition : ScriptableObject
{
    [Tooltip("Orden de aplicación. Menor = antes.")]
    [SerializeField] private int priority = 0;

    public int Priority
    {
        get { return priority; }
    }

    /// <summary>
    /// It is called every attack to modify the context.
    /// Modify the attack context.
    /// </summary>
    /// <param name="ctx"> Attack context </param>
    public abstract void ModifyAttack(ref AttackContext ctx);
}