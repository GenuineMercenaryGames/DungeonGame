using UnityEngine;

public class AttackOrchestrator : MonoBehaviour
{
    [SerializeField] private PlayerItemSystem itemSystem;

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    TryAttack();
        //}
    }

    public void TryAttack()
    {
        WeaponDefinitionBase weapon = itemSystem.equippedWeapon;
        
        if(weapon == null || weapon.AttackDefinition == null)
        {
            Debug.LogWarning("ERROR: Not valid weapon equipped");
            return;
        }

        AttackContext ctx = default;
        weapon.AttackDefinition.BuildBaseContext(weapon, ref ctx);

        foreach(PassiveWeaponModuleDefinition module in itemSystem.ActiveModules)
        {
            module.ModifyAttack(ref ctx);
        }

        Vector3 aim = transform.forward;
        weapon.AttackDefinition.Execute(transform, aim, in ctx);
    }
}