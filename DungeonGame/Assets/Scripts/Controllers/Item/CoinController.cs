using UnityEngine;

// NOTE : Maybe I should implement a generic pickup-able item interface of sorts? or some sort of inheritance bullshit.
// Not sure, I believe that this is good as it is, with a different component for each item type, or maybe this being configurable or whatever, but who knows.
// We'll see what's best later on.
public class CoinController : MonoBehaviour
{
    [SerializeField] private int value;

    void OnTriggerEnter(Collider other)
    {
        // TODO : Implement pickup logic.
    }

    private void DestroyCoin()
    {
        // TODO : Modify logic once coin object pooling is implemented.
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
