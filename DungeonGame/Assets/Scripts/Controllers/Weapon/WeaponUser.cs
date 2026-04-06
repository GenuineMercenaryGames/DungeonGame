using UnityEngine;
using UnityEngine.Events;

public class WeaponUser : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Transform bulletSpawnTransform;
    [SerializeField] public WeaponController weaponController;
    [SerializeField] public EntityTeamController teamController;

    [Header("Events")]
    [SerializeField] public UnityEvent OnShootNotify;

    public void ShootBegin()
    {
        weaponController.AttackPressed();
    }

    public void ShootEnd()
    {
        weaponController.AttackReleased();
    }

    public void ShootNotify()
    {
        OnShootNotify?.Invoke();
    }
}
