using UnityEngine;

public class PlayerGravity : MonoBehaviour
{
    [SerializeField] public Vector3 Gravity;

    private PlayerMovement playerMovement;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        playerMovement.AddVelocity(Gravity);
    }
}
