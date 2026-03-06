using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshController : MonoBehaviour
{

    [SerializeField] private NavMeshSurface navMeshSurface;

    public void Regenerate()
    {
        navMeshSurface.BuildNavMesh();
    }
}
