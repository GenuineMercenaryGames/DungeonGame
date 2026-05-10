using UnityEngine;

// U98 Dispersion pistol hack lol
public class AmmoRegenerator : MonoBehaviour
{
    [SerializeField] public WeaponController Weapon;
    [SerializeField] public int RegenAmmount;
    [SerializeField] public float RegenTime;

    private float timeAccumulator;

    void Start()
    {
        timeAccumulator = 0.0f;
    }

    void Update()
    {
        timeAccumulator += Time.deltaTime;

        if (Weapon == null) return;
        if (timeAccumulator < RegenTime) return;

        Weapon.Ammo += RegenAmmount;
        timeAccumulator = 0.0f;
    }
}
