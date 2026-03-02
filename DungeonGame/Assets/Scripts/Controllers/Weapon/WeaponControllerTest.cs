using UnityEngine;
using UnityEngine.Events;

public class WeaponControllerTest : MonoBehaviour
{
    // NOTE : Maybe these settings should go within a weapon data struct of sorts.
    [Header("Settings")]
    [SerializeField] public float BaseDamage;
    [SerializeField] public float BaseTimeBetweenShots;
    [SerializeField] public int Ammo;

    [Header("Events")]
    [SerializeField] public UnityEvent OnShootPressed;
    [SerializeField] public UnityEvent OnShootReleased;
}
