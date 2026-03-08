using UnityEngine;


public class AttackOrchestrator : MonoBehaviour
{
    [SerializeField] private PlayerItemSystem itemSystem;
    [SerializeField] private Transform playerWeaponTansform;

    private ObjectPoolController pool;

    void Start()
    {
        //pool = ObjectPoolManager.Instance.GetObjectPool(bulletPrefab);
    }

    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    TryAttack();
        //}
    }

    public bool TryAttack()
    {
        WeaponDefinitionBase weapon = itemSystem.equippedWeapon;
        
        if(weapon == null || weapon.AttackDefinition == null)
        {
            Debug.LogWarning("ERROR: Not valid weapon equipped");
            return false;
        }

        AttackContext ctx = default;
        weapon.AttackDefinition.BuildBaseContext(weapon, ref ctx);

        foreach(PassiveWeaponModuleDefinition module in itemSystem.ActiveModules)
        {
            module.ModifyAttack(ref ctx);
        }

        Vector3 aim = playerWeaponTansform.forward;
        return weapon.AttackDefinition.Execute(playerWeaponTansform, aim, in ctx);
    }
}