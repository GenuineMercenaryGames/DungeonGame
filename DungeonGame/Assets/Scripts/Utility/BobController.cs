using UnityEngine;

public class BobController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Transform bobTarget;

    [Header("Bob Settings")]
    [SerializeField] public Vector3 bobAxis;
    [SerializeField] public float maxBobDistance;

    void Start()
    {
        if(bobTarget == null)
            bobTarget = transform;
    }

    void Update()
    {
        bobTarget.position = transform.position + bobAxis.normalized * Mathf.Sin(Time.time) * maxBobDistance;
    }
}
