using UnityEngine;

[CreateAssetMenu]
public class WeaponDataSO : ScriptableObject
{
    public string Name;
    public float Damage;
    public float AttackRate;
    public GameObject ProjectilePrefab;
}
