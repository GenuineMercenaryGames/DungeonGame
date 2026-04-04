using UnityEngine;
using UnityEngine.Events;

public class WeaponController : MonoBehaviour
{
    // NOTE : If someone dislikes this composition model, we can always very trivially change this to use an interface and ScriptableObjects.
    // This is here just for flexibility for now, but once the different weapon types are more known, we can fixate the system to something more specific.
    // Chill the fuck out and leave my inbox alone for a day or two, ok? will ya get off my case?

    [Header("Components")]
    [SerializeField] public Transform bulletSpawnTransform; // This is given here for flexibility for NPC creation, but for the player, this is a disgusting hack and I hate it. This data should not be exposed like that.

    [Header("Events")]
    [SerializeField] public UnityEvent OnShootPressed;
    [SerializeField] public UnityEvent OnShootReleased;
    [SerializeField] public UnityEvent OnShootTick;

    public GameObject owner;
    public bool IsShooting { get; private set; }

    void Start()
    {
        IsShooting = false;
    }

    void Update()
    {
        AttackTick();
    }

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












    // NOTE : Disabled. This is the old code. This function should be removed.
    // Weapons have logic for when the attack begins, when the attack ends, and when the attack ticks. Just one single function wasn't gonna cut it, so I got rid of it.
    // This remains here to allow old code to compile, but this should be killed off eventually. Please, for the love of God, stop touching this fucking retarded
    // function whose purpose is no more. The function has already reached its end of life, let it rest in fucking piss already.
    public bool Attack()
    {
        //// ROF check
        //if (elapsedTime < timeBetweenShots)
        //    return false;
        //elapsedTime = 0.0f;
        //
        //// Spawn the bullet
        //// var obj = Instantiate(bulletPrefab);
        //var bullet = pool.Get<BulletController>();
        //bullet.transform.position = bulletSpawnTransform.position;
        //bullet.transform.rotation = bulletSpawnTransform.rotation;
        //bullet.Owner = gameObject;
        //bullet.Init(); // Re-init con la llamada de Init desde Awake()?

        // Notify success
        return true;
    }
}
