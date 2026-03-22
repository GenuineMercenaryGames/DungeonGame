using Assets.Scripts.ScriptableObjects;
using System;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;


namespace Assets.Scripts.Generation
{
    public enum DungeonType
    {
        FOREST,
        DESERT
    }

    public enum CellType
    {
        NONE,
        HALLWAY,
        ROOM
    }

    public class World : MonoBehaviour
    {
        public Vector2Int MaxWorldSizeInCells { get { return maxWorldSizeInCells; } }
        [SerializeField] private Vector2Int maxWorldSizeInCells;
        [SerializeField] private int cellsPerChunk;
        [SerializeField] private DungeonGenerator[] dungeonGenerators;
        [SerializeField] private Transform player;
        [SerializeField] private int playerChunkVisibleRange;

        private Generator2D _generator;

        private Vector2Int _chunksPerAxis;
        private Chunk[] _chunks;

        private System.Random _random;

        public void SetCell(Vector2Int pos, CellType cell)
        {
            GetChunk(pos).SetCell(GetLocalPos(pos), cell);
        }

        public CellType GetCell(Vector2Int pos) 
        {
            return GetChunk(pos).GetCell(GetLocalPos(pos));
        }

        public Chunk GetChunk(Vector2Int pos)
        {
            return _chunks[GetIndexFromPos(pos)];
        }

        private Vector2Int GetLocalPos(Vector2Int pos)
        {
            return new Vector2Int(pos.x % cellsPerChunk, pos.y % cellsPerChunk);
        }

        private int GetIndexFromPos(Vector2Int pos)
        {
            Vector2Int chunkCoord = GetChunkCoord(new Vector3(pos.x, 0.0f, pos.y));
            return chunkCoord.x * _chunksPerAxis.y + chunkCoord.y;
        }

        private int GetIndexFromChunkCoord(Vector2Int c)
        {
            return c.x * _chunksPerAxis.y + c.y;
        }

        private Vector3 GetWorldPosFromIndex(int id)
        {
            return new Vector3((id / _chunksPerAxis.y) * cellsPerChunk, 0.0f, (id % _chunksPerAxis.y) * cellsPerChunk);
        }
        private Vector2Int GetChunkCoord(Vector3 globalPos)
        {
            return new Vector2Int(((int)globalPos.x / cellsPerChunk), ((int)globalPos.z / cellsPerChunk));
        }

        private bool IsChunkCoordInWorldBounds(Vector2Int p)
        {
            return p.x >= 0 && p.x < _chunksPerAxis.x && p.y >= 0 && p.y < _chunksPerAxis.y;
        }

        void Awake()
        {
            _random = new System.Random((int)DateTime.Now.Ticks);
            _chunksPerAxis = MaxWorldSizeInCells / cellsPerChunk;
            _chunks = new Chunk[_chunksPerAxis.x * _chunksPerAxis.y];
            for(int i = 0; i < _chunks.Length; i++) 
            {
                _chunks[i] = new Chunk(GetWorldPosFromIndex(i), cellsPerChunk, this);
            }
            _generator = new Generator2D(this, _random);
        }

        private void Start()
        {
            GenerateDungeon(DungeonType.FOREST);
        }

        private void LateUpdate()
        {
            for (int x = -playerChunkVisibleRange; x <= playerChunkVisibleRange; x++)
            {
                for (int y = -playerChunkVisibleRange; y <= playerChunkVisibleRange; y++)
                {
                    Vector2Int playerChunkCoord = GetChunkCoord(player.transform.position);
                    Vector2Int pos = new Vector2Int(x, y) + playerChunkCoord;

                    if(IsChunkCoordInWorldBounds(pos))
                    {
                        _chunks[GetIndexFromChunkCoord(pos)].RenderChunk();
                    }
                }
            }
        }

        public void GenerateDungeon(DungeonType type) 
        {
            DungeonGenerator gen = dungeonGenerators[(int)type];
            _generator.Generate(gen);
            for (int i = 0; i < _chunks.Length; i++)
            {
                _chunks[i].PopulateChunk(gen);
            }
        }

        public System.Random GetRandom()
        {
            return _random;
        }

        private void OnDrawGizmosSelected()
        {
            return;
            if (_chunks == null) return;
            _chunks[0].DrawGizmos();
            _generator?.DrawGizmos();
            for (int i = 0; i < _chunks.Length; i++)
            {
                //_chunks[i].DrawGizmos();
            }
        }
    }
}