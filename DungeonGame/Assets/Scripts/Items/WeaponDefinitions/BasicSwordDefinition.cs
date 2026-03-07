using UnityEngine;

[CreateAssetMenu(menuName = "Roguelike/Weapons/Basic Sword")]
public class BasicSwordDefinition : WeaponDefinitionBase
{
    [Header("Sword Extras")]
    [SerializeField] private float hitStop = 0.06f;
    [SerializeField] private AudioClip swingSfx;

    [SerializeField] GameObject bulletPrefab;

    // AMMO

    public float HitStop
    {
        get { return hitStop; }
    }
    public AudioClip SwingSfx
    {
        get { return swingSfx; }
    }
}