using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using System;
using Assets.Scripts.Generation.DungeonGeneration.Utils;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Generation;
using Rect = Assets.Scripts.Generation.Rect;
using UnityEngine.LightTransport;

public struct DoorSegment
{
    public Vector3 Start;
    public Vector3 End;

    public ushort RoomId;

    public DoorSegment(Vector3 start, Vector3 end)
    {
        Start = start;
        End = end;
        RoomId = 0;
    }
}
public class Generator2D {

    private DungeonGenerator _generator;

    private const int PARENT_ROOM_NULL = 0;
    private const int MAX_RECT_PER_ROOM_COUNT = 32;

    private World _world;

    DynamicArray<Rect> _roomRects;
    DynamicArray<Room> _rooms;

    Random _random;
    Delaunay2D delaunay;
    HashSet<Prim.Edge> selectedEdges;

    List<Vertex> vertices;
    List<Prim.Edge> edges;

    private int _offset = 64;


    public Generator2D(World world, Random random, int maxRoomCount, DynamicArray<Room> rooms) {
        _roomRects = new DynamicArray<Rect>(maxRoomCount);
        vertices = new List<Vertex>();
        edges = new List<Prim.Edge>();
        _world = world;
        _random = random;
        _rooms = rooms;
    }

    public void Generate(DungeonGenerator generator) {
        _generator = generator;
        
        _roomRects.Clear();
        _rooms.Clear();

        // Layout generation
        PlaceRooms();
        Triangulate();
        CreateHallways();
        PathfindHallways();

        for(int i = 0; i < _rooms.Count; i++) 
        { 
            for(int j = 0; j < _rooms[i].RectCount; j++)
            {
                foreach (var p in _rooms[i].Rects[j].bounds.allPositionsWithin)
                {
                    _world.SetCell(p, (ushort)(i + World.CELL_TYPE_ROOM));
                }
            }
        }
    }

    void PlaceRooms() {
        for (int i = 0; i < _generator.roomCount; i++) {
            Vector2Int location = new Vector2Int(
                _random.Next(0, _world.MaxDungeonSizeInCells.x) + _offset,
                _random.Next(0, _world.MaxDungeonSizeInCells.y) + _offset
            );

            Vector2Int roomSize = new Vector2Int(
                _random.Next(_generator.roomMinSize.x, _generator.roomMaxSize.x + 1),
                _random.Next(_generator.roomMinSize.y, _generator.roomMaxSize.y + 1)
            );

            bool add = true;
            Rect newRoom = new Rect(location, roomSize, PARENT_ROOM_NULL);

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= _world.MaxDungeonSizeInCells.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= _world.MaxDungeonSizeInCells.y) {
                add = false;
            }

            if (add) {
                _roomRects.Add(newRoom);

                foreach (var pos in newRoom.bounds.allPositionsWithin)
                {
                    _world.SetCell(pos, World.CELL_TYPE_ROOM);
                }
            }
        }
        // --- Union-Find helpers ---
        int[] parent = new int[_roomRects.Count];
        for (int i = 0; i < parent.Length; i++) parent[i] = i;

        int Find(int x)
        {
            while (parent[x] != x) { parent[x] = parent[parent[x]]; x = parent[x]; }
            return x;
        }
        void Union(int a, int b)
        {
            a = Find(a); b = Find(b);
            if (a != b) parent[b] = a;
        }

        // --- Merge all intersecting rects into groups ---
        for (int i = 0; i < _roomRects.Count; i++)
        {       
            for (int j = i + 1; j < _roomRects.Count; j++)
            {
                if (Rect.Intersect(_roomRects[i], _roomRects[j]))
                    Union(i, j);
            }
        }

        // --- Build rooms from groups ---
        Dictionary<int, int> rootToRoom = new();

        for (int i = 0; i < _roomRects.Count; i++)
        {
            int root = Find(i);
            if (!rootToRoom.TryGetValue(root, out int roomIndex))
            {
                roomIndex = _rooms.Count;
                _rooms.Add(new Room(MAX_RECT_PER_ROOM_COUNT, _world));
                rootToRoom[root] = roomIndex;
            }
            _rooms[roomIndex].AddRect(_roomRects[i]);
            _roomRects[i].ParentRoom = roomIndex;
        }
    }

    public void DrawRoomRectsGizmos()
    {
        if(!Application.isPlaying || _rooms == null || _rooms.Count == 0)
        {
            return;
        }
        for(int i = 0; i < _rooms.Count; i++)
        {
            float c = (i * _rooms.Count / 256.0f);
            Gizmos.color = new Color(c, 0, 0);
            for(int r = 0; r < _rooms[i].RectCount; r++)
            {
                if (_rooms[i].Rects == null) return;
                Rect rect = _rooms[i].Rects[r];
                Gizmos.DrawCube(new Vector3(rect.bounds.center.x, 0.0f, rect.bounds.center.y), new Vector3(rect.bounds.size.x, 1.0f, rect.bounds.size.y));
            }
        }
    }

    Vector2 ComputeCentroid(Room room)
    {
        float totalArea = 0f;
        Vector2 centroid = Vector2.zero;

        for (int i = 0; i < room.RectCount; i++)
        {
            Rect r = room.Rects[i];

            float area = r.bounds.size.x * r.bounds.size.y;
            Vector2 center = r.bounds.center;

            centroid += center * area;
            totalArea += area;
        }

        if (totalArea > 0)
            centroid /= totalArea;

        return centroid;
    }

    void Triangulate() {
        vertices.Clear();

        for(int i = 0; i < _rooms.Count; i++)
        {
            Vector2 centroid = ComputeCentroid(_rooms[i]);
            vertices.Add(new Vertex(centroid));
        }
        /*
        for (int i = 0; i < _roomRects.Count; i++) 
        {
            Rect rect = _roomRects.Get(i);
            vertices.Add(new Vertex<Rect>((Vector2)rect.bounds.position + ((Vector2)rect.bounds.size) / 2, rect));
        }*/

        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void CreateHallways() {
        edges.Clear();

        foreach (var edge in delaunay.Edges) {
            edges.Add(new Prim.Edge(edge.U, edge.V));
        }

        List<Prim.Edge> mst = Prim.MinimumSpanningTree(edges, edges[0].U);

        selectedEdges = new HashSet<Prim.Edge>(mst);
        var remainingEdges = new HashSet<Prim.Edge>(edges);
        remainingEdges.ExceptWith(selectedEdges);

        foreach (var edge in remainingEdges) {
            if (_random.NextDouble() < 0.125 && !selectedEdges.Contains(new Prim.Edge(edge.V, edge.U))) {
                selectedEdges.Add(edge);
            }
        }
    }

    void PathfindHallways() {
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(_world.MaxDungeonSizeInCells);

        foreach (var edge in selectedEdges) {
            var startRoom = edge.U.Position;
            var endRoom = edge.V.Position;

            Vector2 startPosf = startRoom;
            Vector2 endPosf = endRoom;
            Vector2Int startPos = new Vector2Int((int)startPosf.x, (int)startPosf.y);
            Vector2Int endPos = new Vector2Int((int)endPosf.x, (int)endPosf.y);

            var path = aStar.FindPath(startPos, endPos, (DungeonPathfinder2D.Node a, DungeonPathfinder2D.Node b) => {
                DungeonPathfinder2D.PathCost pathCost = new DungeonPathfinder2D.PathCost();
                
                pathCost.cost = Vector2Int.Distance(b.Position, endPos);

                if (_world.GetCell(b.Position) >= World.CELL_TYPE_ROOM) {
                    pathCost.cost += 10;
                } else if (_world.GetCell(b.Position) == World.CELL_TYPE_EMPTY) {
                    pathCost.cost += 5;
                } else if (_world.GetCell(b.Position) == World.CELL_TYPE_HALLWAY) {
                    pathCost.cost += 1;
                }

                pathCost.traversable = true;

                return pathCost;
            });

            if (path != null) {
                for (int i = 0; i < path.Count; i++) {
                    Vector2Int current = path[i];
                    Rect newRoom = new Rect(current, new Vector2Int(10, 10), PARENT_ROOM_NULL);
                    foreach (Vector2Int pos in newRoom.bounds.allPositionsWithin)
                    {
                        _world.SetCell(pos, World.CELL_TYPE_HALLWAY);
                    }

                    if (i > 0) {
                        Vector2Int prev = path[i - 1];

                        Vector2Int delta = current - prev;
                    }
                }
            }
        }
    }

    public List<DoorSegment> GetDoors()
    {
        List<DoorSegment> doors = new List<DoorSegment>();
        HashSet<Vector2Int> visited = new HashSet<Vector2Int>();

        Vector2Int size = _world.MaxDungeonSizeInCells;

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                Vector2Int pos = new Vector2Int(x, y);

                if (visited.Contains(pos))
                    continue;
                ushort roomId = 0;

                if (!IsDoorCell(pos, out roomId))
                    continue;

                // Flood fill contiguous door cells
                List<Vector2Int> segment = FloodFillDoor(pos, visited);

                if (segment.Count == 0)
                    continue;

                // Compute start/end
                DoorSegment door = ComputeSegment(segment);
                
                // Discard door if it's too small
                if(Vector3.SqrMagnitude(door.Start - door.End) <= 1.5f * 1.5f)
                {
                    continue;
                }
                door.RoomId = roomId;
                doors.Add(door);
            }
        }

        return doors;
    }

    bool IsDoorCell(Vector2Int pos, out ushort roomId)
    {
        ushort cell = _world.GetCell(pos);

        // Must be inside a hallway
        if (cell != World.CELL_TYPE_HALLWAY)
        {
            roomId = 0;
            return false;
        }
        // Must touch room
        foreach (Vector2Int dir in Directions4)
        {
            Vector2Int n = pos + dir;
            if (!IsInside(n)) continue;

            ushort cellId = _world.GetCell(n);
            if (cellId >= World.CELL_TYPE_ROOM)
            {
                roomId = cellId;
                return true;
            }
        }
        roomId = 0;
        return false;
    }

    List<Vector2Int> FloodFillDoor(Vector2Int start, HashSet<Vector2Int> visited)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        Queue<Vector2Int> queue = new Queue<Vector2Int>();

        queue.Enqueue(start);
        visited.Add(start);

        Vector2Int doorDir = Vector2Int.zero;
        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            result.Add(current);

            if(doorDir == Vector2Int.zero)
            {
                foreach (Vector2Int dir in Directions4)
                {
                    Vector2Int next = current + dir;

                    if (visited.Contains(next)) continue;
                    if (!IsInside(next)) continue;
                    ushort rid = 0;
                    if (!IsDoorCell(next, out rid)) continue;
                    doorDir = dir;
                    visited.Add(next);
                    queue.Enqueue(next);
                    break;
                }
            } else
            {
                Vector2Int next = current + doorDir;

                if (visited.Contains(next)) continue;
                if (!IsInside(next)) continue;
                ushort rid = 0;
                if (!IsDoorCell(next, out rid)) continue;

                visited.Add(next);
                queue.Enqueue(next);
            }
            
        }

        return result;
    }

    DoorSegment ComputeSegment(List<Vector2Int> cells)
    {
        // Determine orientation
        bool horizontal = true;

        int firstY = cells[0].y;
        foreach (var c in cells)
        {
            if (c.y != firstY)
            {
                horizontal = false;
                break;
            }
        }

        Vector2Int min = cells[0];
        Vector2Int max = cells[0];

        foreach (var c in cells)
        {
            if (horizontal)
            {
                if (c.x < min.x) min = c;
                if (c.x > max.x) max = c;
            }
            else
            {
                if (c.y < min.y) min = c;
                if (c.y > max.y) max = c;
            }
        }

        return new DoorSegment(
            new Vector3(min.x - 0.5f, 0, min.y - 0.5f),
            new Vector3(max.x - 0.5f, 0, max.y - 0.5f)
        );
    }

    static readonly Vector2Int[] Directions4 = new Vector2Int[]
    {
        Vector2Int.up,
        Vector2Int.down,
        Vector2Int.left,
        Vector2Int.right
    };

    bool IsInside(Vector2Int p)
    {
        return p.x >= 0 && p.y >= 0 &&
               p.x < _world.MaxDungeonSizeInCells.x &&
               p.y < _world.MaxDungeonSizeInCells.y;
    }
}
