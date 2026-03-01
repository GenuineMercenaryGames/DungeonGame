using UnityEngine;

public class ItemMagnet : MonoBehaviour
{
    [SerializeField] private float pullForce;

    public float PullForce { get { return pullForce; } }

    void OnTriggerStay(Collider other)
    {
        var item = other.GetComponent<AttractableItem>();
        if (item == null)
            return;
        Vector3 dir = (transform.position - other.transform.position).normalized;
        item.RigidBody.AddForce(dir * pullForce * Time.deltaTime);
    }
}
