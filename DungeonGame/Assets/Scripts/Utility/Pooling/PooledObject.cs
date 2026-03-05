using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPoolStorage OwningPool;
    public void Return()
    {
        if (OwningPool != null)
        {
            // Return to owning pool.
            OwningPool.Return(gameObject);
        }
        else
        {
            // Fallback logic for objects that were instantiated manually rather than using a pool.
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
