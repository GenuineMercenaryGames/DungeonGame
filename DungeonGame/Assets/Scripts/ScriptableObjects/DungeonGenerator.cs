using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DungeonGenerator")]
    public class DungeonGenerator : ScriptableObject
    {
        public int roomCount;
        public Vector2Int roomMaxSize;
        public Vector2Int roomMinSize;

        [Header("Tree")]
        [Tooltip("Set tree probability of spawning. [0, 100]")]
        public int TreeDensity;
        public Material TreeMaterial;
        public Mesh[] DecorationPrefabs;

        public RenderParams RParamsTrees;


        [Header("Floor")]
        public Mesh floorPlane;
        public Material floorMaterial;

        public RenderParams RParamsFloor;


        [Header("Spawn")]
        public GameObject[] PrefabsToSpawn;
        public float[] PrefabsProbabilities;


        public float ChestRoomProbability;
        public GameObject ChestPrefab;

        private void OnValidate()
        {
            RParamsTrees = new RenderParams(TreeMaterial);
            RParamsTrees.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            RParamsTrees.receiveShadows = true;

            RParamsFloor = new RenderParams(floorMaterial);
            RParamsFloor.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            RParamsFloor.receiveShadows = true;
        }
    }
}