using UnityEngine;

public class AttractableItem : MonoBehaviour
{
    private Rigidbody rb;

    public Rigidbody RigidBody { get { return rb; } }

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
}
