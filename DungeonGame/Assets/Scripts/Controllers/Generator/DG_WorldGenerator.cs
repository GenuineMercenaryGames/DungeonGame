using Unity.AI.Navigation;
using UnityEngine;

public class DG_WorldGenerator : MonoBehaviour
{
    [SerializeField] private NavMeshSurface navMesh;

    [SerializeField] private int worldSizeX;
    [SerializeField] private int worldSizeY;

    [SerializeField] private int roomsToSpawn;

    private float tileSize = 30.0f;

    private int[] tiles;

    void Start()
    {
        
    }
}
