using System.Collections.Generic;
using Assets.Scripts.Generation;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshController : MonoBehaviour
{

    [SerializeField] private NavMeshSurface navMeshSurface;

    public Mesh fullWalkablePlane;
    public Transform fullWalkablePlaneTransform;

    public void Awake()
    {
        //navMeshSurface = new NavMeshSurface();
    }

    // Combine all chunk planes and assign it to the navmesh.
    public void Regenerate(Chunk[] chunks)
    {

        List<CombineInstance> combine = new();

        for (int i = 0; i < chunks.Length; i++)
        {
            Mesh walkableMesh = chunks[i].WalkablePlane;
            if (walkableMesh == null || walkableMesh.vertexCount == 0)
            {
                continue;
            }

            combine.Add(new CombineInstance
            {
                mesh = walkableMesh,
                subMeshIndex = 0,
                transform = transform.worldToLocalMatrix
            });
        }

        fullWalkablePlane = new Mesh();
        fullWalkablePlane.name = "combinedWalkableMesh";
        fullWalkablePlane.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        fullWalkablePlane.CombineMeshes(combine.ToArray(), true, true);
        fullWalkablePlane.RecalculateBounds();

        GameObject walkable = new GameObject("combinedWalkable");
        walkable.transform.SetParent(transform, false);
        fullWalkablePlaneTransform = walkable.transform;

        MeshCollider meshCollider = walkable.AddComponent<MeshCollider>();

        meshCollider.sharedMesh = fullWalkablePlane;
        
        navMeshSurface.BuildNavMesh();
    }
}
