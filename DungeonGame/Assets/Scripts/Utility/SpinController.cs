using UnityEngine;

public class SpinController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] public Transform spinTarget;

    [Header("Spin Settings")]
    [SerializeField] public Vector3 spinAxis;
    [SerializeField] public float spinSpeed;

    void Start()
    {
        if(spinTarget == null)
            spinTarget = transform;
    }

    void Update()
    {
        spinTarget.Rotate(spinAxis.normalized, spinSpeed * Time.deltaTime);
    }
}
