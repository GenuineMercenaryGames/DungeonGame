using UnityEngine;

public enum AttackMode
{
    Single,
    Auto,
}

public struct WeaponDataEntry
{
    public float Damage;
    public float DelayBetweenShots;
    public AttackMode AttackMode;
}

[System.Serializable]
public struct WeaponData
{
    WeaponDataEntry Primary;
    WeaponDataEntry Secondary;
}
