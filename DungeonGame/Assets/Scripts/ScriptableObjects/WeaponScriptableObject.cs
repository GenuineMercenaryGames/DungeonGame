using UnityEngine;

[CreateAssetMenu]
public class WeaponData : ScriptableObject
{
    public string Name;
    public float Damage;
    public float AttackRate;
    public GameObject ProjectilePrefab;
}
