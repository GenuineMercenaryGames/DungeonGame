using System;
using System.Collections.Generic;
using Assets.Scripts.Generation.DungeonGeneration.Utils;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;

//using UnityEngine.Rendering; Esto te lo quito porque me da name clash con el dynamic array de tu Utils. - kike
// using static UnityEditor.PlayerSettings;


namespace Assets.Scripts.Generation
{
    public enum DungeonType
    {
        FOREST,
        DESERT
    }

    public enum RoomType
    {
        DEFAULT,
        PLAYER_SPAWN,
        BOSS,
        DENSE_FOREST,
        CHEST
    }

    public class Room
    {
        public int RectCount { get { return _rectCount; } }
        public Rect[] Rects;
        private int _rectCount;
        private World _world;
        public bool AlreadyCleared;
        // TODO: make private with a proper getter method
        public List<Enemy> _enemies = new List<Enemy>();
        private List<GameObject> _doors = new List<GameObject>();

        public RoomType RoomType;

        public bool HasSpawnedContents { get; set; }

        public int EnemyCount;

        public Room(int maxRectCount, World world)
        {
            Rects = new Rect[maxRectCount];
            _rectCount = 0;
            HasSpawnedContents = false;
            RoomType = RoomType.DEFAULT;
            EnemyCount = 0;
            _world = world;
            AlreadyCleared = false;
        }

        public void AddDoor(GameObject door)
        {
            _doors.Add(door);
        }

        public List<GameObject> GetDoors()
        {
            return _doors;
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            EnemyCount++;
            enemy.OnDie += EnemyDied;
        }

        public void AddRect(Rect a)
        {
            Rects[_rectCount++] = a;
        }

        private void EnemyDied(Enemy enemy)
        {
            _enemies.Remove(enemy);
            EnemyCount--;
            if(EnemyCount <= 0)
            {
                _world.RoomCleared(this);
                AlreadyCleared = true;
            }
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

    [Serializable]
    public struct GizmosInfo
    {
        public bool RoomTypes;
        public bool CellTypes;
        public bool RoomRects;
        public bool Doors;
        public bool Hallways;
    }

    public class World : MonoBehaviour
    {
        // Constants
        public const ushort CELL_TYPE_ROOM = 2;
        public const ushort CELL_TYPE_HALLWAY = 1;
        public const ushort CELL_TYPE_EMPTY = 0;

        // Events
        public event Action<Chunk> OnChunkLoad;
        public event Action<Chunk> OnChunkUnload;

        public event Action<Room> OnRoomEnter;
        public event Action<Room> OnRoomExit;
        public event Action<Room> OnRoomCleared;

        public List<DoorSegment> Doors { get { return _doors; } }

        public Vector2Int MaxWorldSizeInCells { get { return maxWorldSizeInCells; } }
        public Vector2Int MaxDungeonSizeInCells { get { return maxDungeonSizeInCells; } }
        public Vector3 PlayerSpawnPosition { get { return _playerSpawnPosition; } }

        [SerializeField] private int seed = 0;
        [SerializeField] private Vector2Int maxWorldSizeInCells;
        [SerializeField] private Vector2Int maxDungeonSizeInCells;
        [SerializeField] private int cellsPerChunk;
        [SerializeField] private DungeonGenerator[] dungeonGenerators;
        [SerializeField] private Transform player;
        [SerializeField] private int playerChunkVisibleRange;
        [SerializeField] private NavMeshController navMeshController;



        private Generator2D _generator;

        private Vector2Int _chunksPerAxis;
        private Chunk[] _chunks;

        private System.Random _random;

        DynamicArray<Room> _rooms;
        public DynamicArray<Room> Rooms { get { return _rooms; } }

        private const int MAX_ROOM_COUNT = 256;

        private Vector3 _playerSpawnPosition;
        private ushort _lastPlayerCell = 0;


        List<DoorSegment> _doors;

        private Vector2Int _lastPlayerChunkCoord = Vector2Int.zero;

        [SerializeField] private GizmosInfo gizmosInfo;


        public void RoomCleared(Room room)
        {
            OnRoomCleared?.Invoke(room);
        }

        public void SetCell(Vector2Int pos, ushort cell)
        {
            GetChunk(pos).SetCell(GetLocalPos(pos), cell);
        }

        public ushort GetCell(Vector2Int pos) 
        {
            return GetChunk(pos).GetCell(GetLocalPos(pos));
        }

        public Room GetRoomAtCell(Vector2Int cell)
        {
            ushort cid = GetCell(cell);
            if (cid >= CELL_TYPE_ROOM)
            {
                return _rooms[cid - CELL_TYPE_ROOM];
            } else
            {
                return null;
            }
        }

        public Room GetRoom(ushort roomId)
        {
            if (roomId >= CELL_TYPE_ROOM)
            {
                return _rooms[roomId - CELL_TYPE_ROOM];
            }
            else
            {
                return null;
            }
        }

        public Chunk GetChunk(Vector2Int pos)
        {
            return _chunks[GetIndexFromPos(pos)];
        }

        public void AddGameObjectAtChunk(Vector3 pos, GameObject go)
        {
            GetChunk(new Vector2Int((int)pos.x, (int)pos.z)).AddGameObject(go);
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
            if(seed == 0)
            {
                seed = (int)DateTime.Now.Ticks;
            }
            Debug.Log("Seed: " + seed);
            _random = new System.Random(seed);
            _chunksPerAxis = MaxWorldSizeInCells / cellsPerChunk;
            _chunks = new Chunk[_chunksPerAxis.x * _chunksPerAxis.y];
            for(int i = 0; i < _chunks.Length; i++) 
            {
                _chunks[i] = new Chunk(GetWorldPosFromIndex(i), cellsPerChunk, this);
            }
            _rooms = new DynamicArray<Room>(MAX_ROOM_COUNT);
            _generator = new Generator2D(this, _random, MAX_ROOM_COUNT, _rooms);

            GenerateDungeon(DungeonType.FOREST);
        }

        private void Start()
        {

        }

        private void Update()
        {
            Vector2Int playerChunkCoord = GetChunkCoord(player.transform.position);
            if (_lastPlayerChunkCoord == playerChunkCoord) return;


            // Previously loaded
            int l = 0;
            int size = (2 * playerChunkVisibleRange + 1);
            Span<int> previouslyLoaded = stackalloc int[size * size];

            for (int x = -playerChunkVisibleRange; x <= playerChunkVisibleRange; x++)
            {
                for (int y = -playerChunkVisibleRange; y <= playerChunkVisibleRange; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y) + _lastPlayerChunkCoord;

                    if (IsChunkCoordInWorldBounds(pos))
                    {
                        int id = GetIndexFromChunkCoord(pos);
                        previouslyLoaded[l++] = id;
                        _chunks[id].MarkToUnload = true;
                    }
                }
            }
            // To keep or set to load
            _lastPlayerChunkCoord = playerChunkCoord;
            for (int x = -playerChunkVisibleRange; x <= playerChunkVisibleRange; x++)
            {
                for (int y = -playerChunkVisibleRange; y <= playerChunkVisibleRange; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y) + _lastPlayerChunkCoord;

                    if (IsChunkCoordInWorldBounds(pos))
                    {
                        int id = GetIndexFromChunkCoord(pos);
                        _chunks[id].MarkToUnload = false;
                        if (!_chunks[id].IsLoaded)
                        {
                            _chunks[id].LoadChunk();
                        }
                    }
                }
            }

            // Unload remaining ones
            for(int i = 0; i < l; i++) 
            {
                Chunk c = _chunks[previouslyLoaded[i]];
                if(c.MarkToUnload)
                {
                    c.UnloadChunk();
                }
            }
        }

        private void LateUpdate()
        {
            // Check if player changed room
            ushort currentPlayerCell = GetCell(new Vector2Int((int)Math.Floor(player.transform.position.x + 0.5f),
                (int)Math.Floor(player.transform.position.z + 0.5f)));
            if(currentPlayerCell != _lastPlayerCell)
            {
                if (_lastPlayerCell >= CELL_TYPE_ROOM)
                {
                    OnRoomExit?.Invoke(_rooms[_lastPlayerCell - CELL_TYPE_ROOM]);
                }
                if(currentPlayerCell >= CELL_TYPE_ROOM) 
                {
                    OnRoomEnter?.Invoke(_rooms[currentPlayerCell - CELL_TYPE_ROOM]);
                }

                _lastPlayerCell = currentPlayerCell;
            }

            // Render chunks
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
            gen.EnsureRuntimeData();
            _generator.Generate(gen);
            

            // Get player spawn position

            float maxDistSq = 0.0f;
            int roomId = 0;
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
                        roomId = i;
                    }
                }
            }

            _rooms[roomId].RoomType = RoomType.PLAYER_SPAWN;

            // Get boss room
            maxDistSq = 0.0f;
            Vector2 playerSpawnPosVec2 = new Vector2(_playerSpawnPosition.x, _playerSpawnPosition.z);
            roomId = 0;
            for (int i = 0; i < _rooms.Count; i++)
            {
                for (int j = 0; j < _rooms[i].RectCount; j++)
                {
                    Vector2 center = _rooms[i].Rects[j].Center - playerSpawnPosVec2;
                    float distSq = center.SqrMagnitude();
                    if (distSq > maxDistSq)
                    {
                        maxDistSq = distSq;
                        roomId = i;
                    }
                }
            }
            _rooms[roomId].RoomType = RoomType.BOSS;



            // Choose room types
            for (int i = 0; i < _rooms.Count; i++)
            {
                if (_rooms[i].RoomType != RoomType.DEFAULT)
                    continue;

                float r = GetRandom().Next(0, 100) / 100.0f;
                if(r <= gen.ChestRoomProbability)
                {
                    _rooms[i].RoomType = RoomType.CHEST;
                }
                r = GetRandom().Next(0, 100) / 100.0f;
                if (r <= gen.DenseForestRoomProbability)
                {
                    _rooms[i].RoomType = RoomType.DENSE_FOREST;
                }
            }


            _doors = _generator.GetDoors();

            // Populate chunks
            for (int i = 0; i < _chunks.Length; i++)
            {
                _chunks[i].PopulateChunk(gen);
            }

            navMeshController?.Regenerate(_chunks);
        }

        public System.Random GetRandom()
        {
            return _random;
        }

        private void OnDrawGizmos()
        {
            
            if (_doors != null && gizmosInfo.Doors)
            {
                foreach (DoorSegment d in _doors)
                {
                    Gizmos.DrawLine(d.Start, d.End);
                    Gizmos.DrawSphere(d.Start, 0.75f);
                    Gizmos.DrawSphere(d.End, 0.75f);
                }
            }
            
            if (_chunks != null && gizmosInfo.CellTypes)
            {
                for (int x = -playerChunkVisibleRange; x <= playerChunkVisibleRange; x++)
                {
                    for (int y = -playerChunkVisibleRange; y <= playerChunkVisibleRange; y++)
                    {
                        Vector2Int playerChunkCoord = GetChunkCoord(player.transform.position);
                        Vector2Int pos = new Vector2Int(x, y) + playerChunkCoord;
                        if (IsChunkCoordInWorldBounds(pos))
                        {
                            _chunks[GetIndexFromChunkCoord(pos)].DrawGizmosCellTypes();
                        }
                    }
                }
            }  
            
            if(_chunks != null && gizmosInfo.RoomTypes)
            {
                Gizmos.DrawSphere(_playerSpawnPosition, 10.0f);
                for (int i = 0; i < _rooms.Count; ++i)
                {
                    switch (_rooms[i].RoomType)
                    {
                        case RoomType.CHEST:
                            Gizmos.color = Color.yellow; break;
                        case RoomType.BOSS:
                            Gizmos.color = Color.blue; break;
                        case RoomType.PLAYER_SPAWN:
                            Gizmos.color = Color.red; break;
                        default:
                            continue;
                    }
                    Gizmos.DrawSphere(new Vector3(_rooms[i].Rects[0].Center.x, 0.0f, _rooms[i].Rects[0].Center.y), 5.0f);
                }
            }

            if(_chunks != null && gizmosInfo.RoomRects)
            {
                _generator?.DrawRoomRectsGizmos();
            }

            if(_chunks!=null && gizmosInfo.Hallways)
            {
                _generator?.DrawHallways();
            }
        }
    }
}
