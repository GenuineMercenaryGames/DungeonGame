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
        Vector3 oldPos = transform.position;
        Vector3 newPos = new Vector3(oldPos.x, oldPos.y + Mathf.Sin(Time.time) * maxBobDistance, oldPos.z);
        bobTarget.position = newPos;
    }
}
