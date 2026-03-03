using UnityEngine;

public abstract class PassiveWeaponModuleDefinition : ScriptableObject
{
    [Tooltip("Orden de aplicación. Menor = antes.")]
    [SerializeField] private int priority = 0;

    public int Priority
    {
        get { return priority; }
    }

    /// Se llama al recoger el item pasivo (1 vez).
    public virtual void OnAdded(PlayerItemSystem player)
    {
        Debug.Log($"[Module Added] {name}");
    }

    /// Se llama al quitar el pasivo (si lo implementáis).
    public virtual void OnRemoved(PlayerItemSystem player)
    {
        Debug.Log($"[Module Removed] {name}");
    }

    /// <summary>
    /// It is called every attack to modify the context.
    /// Modify the attack context.
    /// </summary>
    /// <param name="ctx"> Attack context </param>
    public abstract void ModifyAttack(ref AttackContext ctx);
}