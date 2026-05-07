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

        [Header("NewSpawn")]
        public GameObject[] TreePrefabs;
        public GameObject[] MeleeEnemyPrefabs;
        public GameObject CoinPrefab;


        public float ChestRoomProbability;
        public float DenseForestRoomProbability;
        public GameObject ChestPrefab;

        public GameObject DoorPrefab;
        public GameObject BossPrefab;
        public GameObject AntennaPrefab;

        public void EnsureRuntimeData()
        {
            if (TreeMaterial == null)
            {
                Debug.LogError($"DungeonGenerator '{name}' is missing TreeMaterial.");
            }
            else
            {
                RParamsTrees = new RenderParams(TreeMaterial)
                {
                    shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On,
                    receiveShadows = true
                };
            }

            if (floorMaterial == null)
            {
                Debug.LogError($"DungeonGenerator '{name}' is missing floorMaterial.");
            }
            else
            {
                RParamsFloor = new RenderParams(floorMaterial)
                {
                    shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off,
                    receiveShadows = true
                };
            }
        }

        private void OnValidate()
        {
            EnsureRuntimeData();
        }
    }
}
