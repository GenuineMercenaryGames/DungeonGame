using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    #region Variables

    [Header("Components")]
    [SerializeField] public WeaponUser weaponUser;

    [Header("Events")]
    [SerializeField] public UnityEvent OnShootPressed;
    [SerializeField] public UnityEvent OnShootReleased;
    [SerializeField] public UnityEvent OnShootTick;

    [Header("Settings")]
    [SerializeField] public float BaseDamage = 10.0f;
    [SerializeField] public float BaseVibration = 4.0f;

    [Header("Sound")]
    [SerializeField] public string shootSound;

    // [Header("Ammo")]
    // [SerializeField] public int MaxAmmo;

    public bool IsShooting { get; private set; }
    // public int Ammo { get; set; }

    #endregion

    #region MonoBehaviour

    void Start()
    {
        IsShooting = false;
        // Ammo = MaxAmmo;
    }

    void Update()
    {
        AttackTick();
    }

    #endregion

    #region PublicMethods

    public void AttackPressed()
    {
        if (IsShooting) return;
        IsShooting = true;
        OnShootPressed.Invoke();
    }

    public void AttackReleased()
    {
        if (!IsShooting) return;
        IsShooting = false;
        OnShootReleased.Invoke();
    }

    public void AttackTick()
    {
        /*
            NOTE : This is probably fucking expensive if we have tons of weapons on the scene.
            It would be far better to do this through the interface + SO idea I mentioned above. But this will suffice for now.
            The idea is to get the implementation rolling because time is running out.
            Also, this function is exposed for flexibility, but it should probably (almost) NEVER be invoked manually from anywhere else.
        */
        if (!IsShooting) return;
        OnShootTick.Invoke();
    }

    #endregion

}
