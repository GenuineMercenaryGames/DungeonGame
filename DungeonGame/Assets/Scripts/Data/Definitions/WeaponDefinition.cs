using UnityEngine;

[CreateAssetMenu(fileName = "NewSweeperWeapon", menuName = "TheSweeper/Weapon")]
public class WeaponDefinition : ScriptableObject
{
    public GameObject WeaponPrefab;
    public string WeaponName;
    public float WeaponDamage;
    public float TimeBetweenShots;
}
