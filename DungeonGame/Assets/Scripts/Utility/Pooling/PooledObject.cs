using UnityEngine;

public class PooledObject : MonoBehaviour
{
    [HideInInspector] public ObjectPool owningPool;
    public void Return()
    {
        if (owningPool != null)
        {
            // Return to owning pool.
            owningPool.Return(gameObject);
        }
        else
        {
            // Fallback logic for objects that were instantiated manually rather than using a pool.
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }
}
