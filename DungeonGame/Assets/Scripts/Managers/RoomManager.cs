using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Generation;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class RoomManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public World _world;
    public DungeonGenerator gen;

    private System.Random _random = new System.Random((int)DateTime.Now.Ticks);

    public void EnableDoors(Room room)
    {
        if(room.AlreadyCleared)
        {
            return;
        }
        foreach (GameObject go in room.GetDoors())
        {
            if (go != null)
                go.SetActive(true);
        }
    }

    public void DisableDoors(Room room)
    {
        foreach (GameObject go in room.GetDoors())
        {
            if (go != null)
                go.SetActive(false);
        }
    }

    private struct YInterval
    {
        public int Min;
        public int Max; // Exclusive

        public YInterval(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }

    private long GetRoomArea(Room room)
    {
        if (room.RectCount == 0)
            return 0;

        List<int> xCoords = new();

        for (int i = 0; i < room.RectCount; ++i)
        {
            RectInt bounds = room.Rects[i].bounds;

            if (bounds.width <= 0 || bounds.height <= 0)
                continue;

            xCoords.Add(bounds.xMin);
            xCoords.Add(bounds.xMax);
        }

        if (xCoords.Count == 0)
            return 0;

        xCoords.Sort();

        long area = 0;
        List<YInterval> yIntervals = new();

        for (int xi = 0; xi < xCoords.Count - 1; ++xi)
        {
            int xMin = xCoords[xi];
            int xMax = xCoords[xi + 1];

            if (xMin == xMax)
                continue;

            yIntervals.Clear();

            for (int i = 0; i < room.RectCount; ++i)
            {
                RectInt bounds = room.Rects[i].bounds;

                if (bounds.xMin <= xMin && bounds.xMax >= xMax)
                    yIntervals.Add(new YInterval(bounds.yMin, bounds.yMax));
            }

            if (yIntervals.Count == 0)
                continue;

            yIntervals.Sort((a, b) => a.Min.CompareTo(b.Min));

            int coveredY = 0;
            int currentMin = yIntervals[0].Min;
            int currentMax = yIntervals[0].Max;

            for (int i = 1; i < yIntervals.Count; ++i)
            {
                YInterval interval = yIntervals[i];

                if (interval.Min <= currentMax)
                {
                    currentMax = Mathf.Max(currentMax, interval.Max);
                }
                else
                {
                    coveredY += currentMax - currentMin;
                    currentMin = interval.Min;
                    currentMax = interval.Max;
                }
            }

            coveredY += currentMax - currentMin;

            area += (long)(xMax - xMin) * coveredY;
        }

        return area;
    }

    private HashSet<Vector2Int> BuildBlockedTreeCells(List<Vector2Int> points, int padding = 1)
    {
        HashSet<Vector2Int> blockedCells = new();

        for (int i = 0; i < points.Count; ++i)
        {
            for (int j = 0; j < points.Count; ++j)
            {
                AddLine(blockedCells, points[i], points[j], padding);
            }
        }

        return blockedCells;
    }

    private void AddLine(HashSet<Vector2Int> blockedCells, Vector2Int start, Vector2Int end, int padding)
    {
        int x0 = start.x;
        int y0 = start.y;
        int x1 = end.x;
        int y1 = end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);

        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;

        int error = dx - dy;

        while (true)
        {
            AddBlockedCellWithPadding(blockedCells, new Vector2Int(x0, y0), padding);

            if (x0 == x1 && y0 == y1)
                break;

            int error2 = error * 2;

            if (error2 > -dy)
            {
                error -= dy;
                x0 += sx;
            }

            if (error2 < dx)
            {
                error += dx;
                y0 += sy;
            }
        }
    }

    private void AddBlockedCellWithPadding(HashSet<Vector2Int> blockedCells, Vector2Int cell, int padding)
    {
        for (int y = -padding; y <= padding; ++y)
        {
            for (int x = -padding; x <= padding; ++x)
            {
                blockedCells.Add(new Vector2Int(cell.x + x, cell.y + y));
            }
        }
    }

    private Vector2Int SampleDoorLines(List<Vector2Int> doorPoints)
    {
        if (doorPoints == null || doorPoints.Count < 2)
            throw new System.InvalidOperationException("Need at least two door points.");

        int i = UnityEngine.Random.Range(0, doorPoints.Count);

        int j = UnityEngine.Random.Range(0, doorPoints.Count - 1);

        if (j >= i)
            ++j;

        float t = UnityEngine.Random.Range(0.3f, 0.7f);

        Vector2Int a = doorPoints[i];
        Vector2Int b = doorPoints[j];

        int x = Mathf.RoundToInt(Mathf.Lerp(a.x, b.x, t));
        int y = Mathf.RoundToInt(Mathf.Lerp(a.y, b.y, t));

        return new Vector2Int(x, y);
    }

    private Vector2Int UniformSampleRoom(Room room)
    {
        if (room.RectCount == 0)
            throw new System.InvalidOperationException("Cannot sample an empty room.");

        RectInt roomBounds = room.Rects[0].bounds;

        for (int i = 1; i < room.RectCount; ++i)
        {
            roomBounds = Union(roomBounds, room.Rects[i].bounds);
        }

        const int maxAttempts = 128;

        for (int attempt = 0; attempt < maxAttempts; ++attempt)
        {
            int x = UnityEngine.Random.Range(roomBounds.xMin, roomBounds.xMax);
            int y = UnityEngine.Random.Range(roomBounds.yMin, roomBounds.yMax);

            Vector2Int point = new Vector2Int(x, y);

            if (ContainsAnyRect(room, point))
                return point;
        }

        Debug.Log("Fallo en el rejection sampling");
        return new Vector2Int(0, 0);
    }

    private static RectInt Union(RectInt a, RectInt b)
    {
        int xMin = Mathf.Min(a.xMin, b.xMin);
        int yMin = Mathf.Min(a.yMin, b.yMin);
        int xMax = Mathf.Max(a.xMax, b.xMax);
        int yMax = Mathf.Max(a.yMax, b.yMax);

        return new RectInt(xMin, yMin, xMax - xMin, yMax - yMin);
    }

    private static bool ContainsAnyRect(Room room, Vector2Int point)
    {
        for (int i = 0; i < room.RectCount; ++i)
        {
            if (room.Rects[i].bounds.Contains(point))
                return true;
        }

        return false;
    }

    private void SpawnDenseForestRoom(Room room)
    {
        float treeDensity = 0.1f;
        int treeCount = (int)(GetRoomArea(room) * treeDensity);

        List<Vector2Int> doorPoints = new List<Vector2Int>();
        foreach(GameObject door in room.GetDoors())
        {
            doorPoints.Add(new Vector2Int((int)(door.transform.position.x), (int)(door.transform.position.z)));
        }

        HashSet<Vector2Int> blockedCells = BuildBlockedTreeCells(doorPoints, 2);

        GameObject roomParentEmpty = new GameObject("Room Parent");

        for (int i = 0; i < treeCount; ++i)
        {
            Vector2Int pos = UniformSampleRoom(room);
            if (blockedCells.Contains(pos))
                continue;
            GameObject gameObject = Instantiate(
                gen.TreePrefabs[0],
                new Vector3(pos.x, 0.0f, pos.y),
                Quaternion.identity
            );
            gameObject.transform.parent = roomParentEmpty.transform;
            gameObject.transform.localScale = Vector3.one + Vector3.one * _random.Next(0, 100000) / 100000.0f;
            _world.AddGameObjectAtChunk(gameObject.transform.position, gameObject);
        }

        if(doorPoints.Count <= 1)
        {
            return;
        }

        float enemyDensity = 0.001f;
        int enemyCount = (int)(GetRoomArea(room) * enemyDensity);

        for (int i = 0; i < enemyCount; ++i)
        {
            Vector2Int pos = SampleDoorLines(doorPoints);

            GameObject gameObject = Instantiate(
                gen.MeleeEnemyPrefabs[0],
                new Vector3(pos.x, 0.0f, pos.y),
                Quaternion.identity
            );
            gameObject.AddComponent<FadeController>();
            room.AddEnemy(gameObject.GetComponent<Enemy>());
            gameObject.SetActive(false);
        }

        float coinDensity = 0.05f;
        for(int i = 0; i < doorPoints.Count; ++i)
        {
            for (int j = 0; j < doorPoints.Count; ++j)
            {
                for(float k = 0.2f; k < 0.8f; k += coinDensity)
                {
                    Vector2 pos = Vector2.Lerp(doorPoints[i], doorPoints[j], k);
                    GameObject gameObject = Instantiate(
                        gen.CoinPrefab,
                        new Vector3(pos.x, 0.5f, pos.y),
                        Quaternion.identity
                    );
                    _world.AddGameObjectAtChunk(gameObject.transform.position, gameObject);
                }
            }
        }
    }

    private void SpawnRoomContents(Room room)
    {

        if (room == null || room.HasSpawnedContents)
        {
            return;
        }


        int prefabCount = Mathf.Min(
            gen.PrefabsToSpawn.Length,
            gen.PrefabsProbabilities.Length
        );

        if (prefabCount == 0)
        {
            room.HasSpawnedContents = true;
            return;
        }

        GameObject roomParentEmpty = new GameObject("Room Parent");

        switch (room.RoomType)
        {
            case RoomType.DEFAULT:
                {
                    for (int i = 0; i < room.RectCount; i++)
                    {
                        RectInt bounds = room.Rects[i].bounds;

                        for (int x = bounds.xMin; x < bounds.xMax; x++)
                        {
                            for (int y = bounds.yMin; y < bounds.yMax; y++)
                            {
                                Vector2Int pos = new Vector2Int(x, y);

                                int p = _random.Next(0, prefabCount);
                                float r = _random.Next(0, 100000) / 100000.0f;

                                // Do not spawn enemies in the boss room or player spawn room.
                                if ((room.RoomType == RoomType.BOSS || room.RoomType == RoomType.PLAYER_SPAWN)
                                    && gen.PrefabsToSpawn[p].TryGetComponent<Enemy>(out _))
                                {
                                    continue;
                                }

                                if (r > gen.PrefabsProbabilities[p])
                                {
                                    continue;
                                }

                                GameObject spawnedObject = Instantiate(
                                    gen.PrefabsToSpawn[p],
                                    new Vector3(pos.x + 0.5f, 0.0f, pos.y + 0.5f),
                                    Quaternion.identity
                                );

                                spawnedObject.transform.parent = roomParentEmpty.transform;

                                Enemy enemy = spawnedObject.GetComponent<Enemy>();

                                if (enemy != null)
                                {
                                    spawnedObject.AddComponent<FadeController>();
                                    room.AddEnemy(enemy);
                                    spawnedObject.SetActive(false);
                                }
                                else
                                {
                                    _world.AddGameObjectAtChunk(spawnedObject.transform.position, spawnedObject);
                                }
                            }
                        }
                    }

                    break;
                }

            case RoomType.DENSE_FOREST:
                {
                    SpawnDenseForestRoom(room);
                    break;
                }

            case RoomType.BOSS:
                {
                    GameObject boss = Instantiate(
                        gen.BossPrefab,
                        new Vector3(room.Rects[0].Center.x, 1.0f, room.Rects[0].Center.y),
                        Quaternion.identity
                    );

                    boss.AddComponent<FadeController>();

                    Enemy enemy = boss.GetComponent<Enemy>();

                    if (enemy != null)
                    {
                        room.AddEnemy(enemy);
                    }
                    else
                    {
                        Debug.LogError("Boss game object does not contain an Enemy component.");
                    }

                    break;
                }

            case RoomType.CHEST:
                {
                    GameObject chest = Instantiate(
                        gen.ChestPrefab,
                        new Vector3(room.Rects[0].Center.x, 1.0f, room.Rects[0].Center.y),
                        Quaternion.Euler(0.0f, 180.0f, 0.0f)
                    );

                    _world.AddGameObjectAtChunk(chest.transform.position, chest);
                    break;
                }
        }

        // Si no se spawnean enemigos, que se quiten las puertas.
        if (room.EnemyCount <= 0)
        {
            room.AlreadyCleared = true;
        }

        room.HasSpawnedContents = true;
    }

    private void EnableRoomEnemies(Room room)
    {
        foreach (Enemy enemy in room._enemies)
        {
            if (enemy.gameObject != null)
            {
                enemy.gameObject.SetActive(true);
                enemy.GetComponent<FadeController>().PlayFadeIn();
            }
        }
    }

    private void DisableRoomEnemies(Room room)
    {
        foreach (Enemy enemy in room._enemies)
        {
            if (enemy.gameObject != null)
                enemy.gameObject.GetComponent<FadeController>().PlayFadeOutAndDisable();
        }
    }


    private Coroutine _enable_doors_coroutine;
    private Room _pending_room;

    private IEnumerator EnableDoorsDelayed(Room room)
    {
        yield return new WaitForSeconds(1.0f);

        if (_pending_room == room)
        {
            EnableDoors(room);
            _enable_doors_coroutine = null;
            _pending_room = null;
        }
    }

    private void StartEnableDoorsTimer(Room room)
    {
        if (_enable_doors_coroutine != null)
        {
            StopCoroutine(_enable_doors_coroutine);
        }
        _pending_room = room;
        _enable_doors_coroutine = StartCoroutine(EnableDoorsDelayed(room));
    }

    private void CancelEnableDoorsTimer(Room room)
    {
        if (_pending_room == room && _enable_doors_coroutine != null)
        {
            StopCoroutine(_enable_doors_coroutine);
            _enable_doors_coroutine = null;
            _pending_room = null;
        }

        DisableDoors(room);
    }

    private void Awake()
    {
        _world.OnRoomEnter += EnableRoomEnemies;
        _world.OnRoomExit += DisableRoomEnemies;

        _world.OnRoomEnter += StartEnableDoorsTimer;
        _world.OnRoomExit += CancelEnableDoorsTimer;
        _world.OnRoomCleared += OnRoomCleared;

    }

    private void OnRoomCleared(Room room)
    {
        SfxManager.Instance.PlaySfx("RoomCleared");

        if(room.RoomType == RoomType.BOSS)
        {
            GameManager.Instance.StartVictory();
        }

        DisableDoors(room);
    }

    void Start()
    {
        // Por ahora spawneamos todas las puertas
        foreach (DoorSegment door in _world.Doors)
        {
            Vector3 dir = door.End - door.Start;
            float length = dir.magnitude;
            Vector3 center = (door.Start + door.End) * 0.5f;

            Quaternion rotation = Quaternion.FromToRotation(Vector3.right, dir.normalized);

            //Vector3 forward = rotation * Vector3.forward;

            GameObject door_go = Instantiate(gen.DoorPrefab, center, rotation);

            Vector3 scale = door_go.transform.localScale;
            scale.x = length + 1f;
            door_go.transform.localScale = scale;
            Room room = _world.GetRoom(door.RoomId);
            door_go.SetActive(false);
            room.AddDoor(door_go);
        }

        for (int i = 0; i < _world.Rooms.Count; ++i)
        {
            Room room = _world.Rooms[i];
            SpawnRoomContents(room);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
