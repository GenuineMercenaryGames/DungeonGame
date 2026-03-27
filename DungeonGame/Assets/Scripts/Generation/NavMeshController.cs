using System.Collections.Generic;
using Assets.Scripts.Generation;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshController : MonoBehaviour
{

    [SerializeField] private NavMeshSurface navMeshSurface;

    public void Awake()
    {
        //navMeshSurface = new NavMeshSurface();
    }

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

        Mesh combinedWalkableMesh = new Mesh();
        combinedWalkableMesh.name = "combinedWalkableMesh";
        combinedWalkableMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        combinedWalkableMesh.CombineMeshes(combine.ToArray(), true, true);
        combinedWalkableMesh.RecalculateBounds();

        GameObject walkable = new GameObject("combinedWalkable");
        walkable.transform.SetParent(transform, false);

        MeshCollider meshCollider = walkable.AddComponent<MeshCollider>();

        meshCollider.sharedMesh = combinedWalkableMesh;

        navMeshSurface.BuildNavMesh();
    }
}
