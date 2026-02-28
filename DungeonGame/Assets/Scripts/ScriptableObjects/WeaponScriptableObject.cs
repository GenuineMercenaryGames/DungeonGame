using UnityEngine;

[CreateAssetMenu(fileName = "NewWeapon", menuName = "Weapons/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public string Name;
    public float Damage;
    public float AttackRate;
    public GameObject ProjectilePrefab;
}
