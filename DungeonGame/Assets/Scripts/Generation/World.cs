using Assets.Scripts.Generation.DungeonGeneration.Utils;
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

    public struct Room
    {
        public int RectCount { get { return _rectCount; } }
        public Rect[] Rects;
        private int _rectCount;

        public Room(int maxRectCount)
        {
            Rects = new Rect[maxRectCount];
            _rectCount = 0;
        }

        public void AddRect(Rect a)
        {
            Rects[_rectCount++] = a;
        }
    }
    public struct Rect
    {
        public RectInt bounds;

        public Vector2 Center { get { return bounds.center; } }

        public int ParentRoom { get; set; }

        public Rect(Vector2Int location, Vector2Int size, int parentRoom)
        {
            bounds = new RectInt(location, size);
            ParentRoom = parentRoom;
        }


        public static bool Intersect(Rect a, Rect b)
        {
            return !((a.bounds.position.x >= (b.bounds.position.x + b.bounds.size.x)) || ((a.bounds.position.x + a.bounds.size.x) <= b.bounds.position.x)
                || (a.bounds.position.y >= (b.bounds.position.y + b.bounds.size.y)) || ((a.bounds.position.y + a.bounds.size.y) <= b.bounds.position.y));
        }
    }

    public class World : MonoBehaviour
    {
        public Vector2Int MaxWorldSizeInCells { get { return maxWorldSizeInCells; } }
        public Vector3 PlayerSpawnPosition { get { return _playerSpawnPosition; } }
        [SerializeField] private Vector2Int maxWorldSizeInCells;
        [SerializeField] private int cellsPerChunk;
        [SerializeField] private DungeonGenerator[] dungeonGenerators;
        [SerializeField] private Transform player;
        [SerializeField] private int playerChunkVisibleRange;

        private Generator2D _generator;

        private Vector2Int _chunksPerAxis;
        private Chunk[] _chunks;

        private System.Random _random;

        DynamicArray<Room> _rooms;

        private const int MAX_ROOM_COUNT = 256;

        private Vector3 _playerSpawnPosition;

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
            _rooms = new DynamicArray<Room>(MAX_ROOM_COUNT);
            _generator = new Generator2D(this, _random, MAX_ROOM_COUNT, _rooms);
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

            // Get player spawn position

            float maxDistSq = 0.0f;
            for(int i = 0; i < _rooms.Count; i++) 
            {
                for(int j = 0; j < _rooms[i].RectCount; j++)
                {
                    Vector2 center = _rooms[i].Rects[j].Center;
                    float distSq = center.SqrMagnitude();
                    if (distSq > maxDistSq)
                    {
                        maxDistSq = distSq;
                        _playerSpawnPosition = new Vector3(center.x, 0.0f, center.y);
                    }
                }
            }
        }

        public System.Random GetRandom()
        {
            return _random;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawSphere(_playerSpawnPosition, 10.0f);
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