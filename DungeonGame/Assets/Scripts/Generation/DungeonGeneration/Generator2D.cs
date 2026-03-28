using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;
using Graphs;
using System;
using Assets.Scripts.Generation.DungeonGeneration.Utils;
using static UnityEditor.FilePathAttribute;
using Assets.Scripts.ScriptableObjects;
using Assets.Scripts.Generation;
using Rect = Assets.Scripts.Generation.Rect;

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
                _random.Next(0, _world.MaxWorldSizeInCells.x),
                _random.Next(0, _world.MaxWorldSizeInCells.y)
            );

            Vector2Int roomSize = new Vector2Int(
                _random.Next(_generator.roomMinSize.x, _generator.roomMaxSize.x + 1),
                _random.Next(_generator.roomMinSize.y, _generator.roomMaxSize.y + 1)
            );

            bool add = true;
            Rect newRoom = new Rect(location, roomSize, PARENT_ROOM_NULL);

            if (newRoom.bounds.xMin < 0 || newRoom.bounds.xMax >= _world.MaxWorldSizeInCells.x
                || newRoom.bounds.yMin < 0 || newRoom.bounds.yMax >= _world.MaxWorldSizeInCells.y) {
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
                _rooms.Add(new Room(MAX_RECT_PER_ROOM_COUNT));
                rootToRoom[root] = roomIndex;
            }
            _rooms[roomIndex].AddRect(_roomRects[i]);
            _roomRects[i].ParentRoom = roomIndex;
        }
    }

    public void DrawGizmos()
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

    void Triangulate() {
        vertices.Clear();

        for (int i = 0; i < _roomRects.Count; i++) 
        {
            Rect rect = _roomRects.Get(i);
            vertices.Add(new Vertex<Rect>((Vector2)rect.bounds.position + ((Vector2)rect.bounds.size) / 2, rect));
        }

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
        DungeonPathfinder2D aStar = new DungeonPathfinder2D(_world.MaxWorldSizeInCells);

        foreach (var edge in selectedEdges) {
            var startRoom = (edge.U as Vertex<Rect>).Item;
            var endRoom = (edge.V as Vertex<Rect>).Item;

            Vector2 startPosf = startRoom.bounds.center;
            Vector2 endPosf = endRoom.bounds.center;
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
}
