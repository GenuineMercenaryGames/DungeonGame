using System.Collections.Generic;
using UnityEngine;
using static WeaponAttackDefinition;

public class PlayerItemSystem : MonoBehaviour
{
    public WeaponDefinitionBase equippedWeapon { get; private set; }            // The player's currently equipped weapon
    private readonly List<PassiveItemDefinitionBase> passiveItems = new();      // List of passive items, they contain the modules that will affect the attack
    private readonly List<PassiveWeaponModuleDefinition> activeModules = new(); // We could obtain the modules from the passive items

    private static ObjectPoolController pool;

    public List<PassiveWeaponModuleDefinition> ActiveModules
    {
        get { return activeModules; }
    }

    public void EquipWeapon(WeaponDefinitionBase weapon)
    {
        equippedWeapon = weapon;

        var weaponDef = equippedWeapon.AttackDefinition as RangedAttackDefinition;
        //if (equippedWeapon.AttackDefinition.AttackKind == AttackKindEnum.RANGED)
        if (weaponDef)
        {
            pool = ObjectPoolManager.Instance.GetObjectPool(((BasicGunDefinition)weapon).bulletPrefab);
        }
        Debug.Log($"Equipped weapon: {equippedWeapon}");
        // Rebuild modules
    }

    public void AddPasiveItem(PassiveItemDefinitionBase passiveItem)
    {
        if(passiveItems.Contains(passiveItem))
        {
            Debug.LogWarning("ERROR: Player already has passive item {" + passiveItem + "}");
            return;
        }

        passiveItems.Add(passiveItem);
        Debug.Log($"Added passive item: {passiveItem}");
        
        foreach(var module in passiveItem.Modules)
        {
            activeModules.Add(module);
            module.OnAdded(this);
        }

        // Order the list of modules according to priority. Items with higher priority will have prefernce when adding functionality to the attack
        activeModules.Sort((a, b) => a.Priority.CompareTo(b.Priority));
    }
}