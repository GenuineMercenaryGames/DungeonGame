using System;
using System.Collections.Generic;
using Assets.Scripts.Generation;
using Assets.Scripts.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using static UnityEditor.PlayerSettings;

public class RoomManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public World _world;
    public DungeonGenerator gen;

    private System.Random _random = new System.Random((int)DateTime.Now.Ticks);
    private List<GameObject> _goDoors = new List<GameObject>();

    public void EnableDoors(Room room)
    {
        foreach (GameObject go in _goDoors)
        {
            if (go != null)
                go.SetActive(true);
        }
    }

    public void DisableDoors(Room room)
    {
        foreach (GameObject go in _goDoors)
        {
            if (go != null)
                go.SetActive(false);
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

                    if (r <= gen.PrefabsProbabilities[p])
                    {
                        GameObject gameObject = Instantiate(
                            gen.PrefabsToSpawn[p],
                            new Vector3(pos.x, 0.0f, pos.y),
                            Quaternion.identity
                        );
                        gameObject.transform.parent = roomParentEmpty.transform;
                        //if (gameObject.GetComponent<NavMeshAgent>() == null)
                        //{
                        //    NavMeshObstacle obstacle = gameObject.AddComponent<NavMeshObstacle>();
                        //    obstacle.carving = true;
                        //}
                        room._instances.Add(gameObject);
                    }
                }
            }
        }

        // Ñapa que he hecho por ahora para spawnear el boss
        if (room.RoomType == RoomType.BOSS)
        {
            GameObject boss = Instantiate(
                gen.BossPrefab,
                new Vector3(room.Rects[0].Center.x, 1, room.Rects[0].Center.y),
                Quaternion.identity
            );
            room._instances.Add(boss);
        }

        if (room.RoomType == RoomType.CHEST)
        {
            GameObject boss = Instantiate(
                gen.ChestPrefab,
                new Vector3(room.Rects[0].Center.x, 1, room.Rects[0].Center.y),
                Quaternion.identity
            );
            room._instances.Add(boss);
        }

        room.HasSpawnedContents = true;
    }

    private void EnableRoomContents(Room room)
    {
        foreach (GameObject go in room._instances)
        {
            if (go != null)
                go.SetActive(true);
        }
    }

    private void DisableRoomContents(Room room)
    {
        foreach (GameObject go in room._instances)
        {
            if (go != null)
                go.SetActive(false);
        }
    }

    private void Awake()
    {
        _world.OnRoomEnter += SpawnRoomContents;
        _world.OnRoomEnter += EnableRoomContents;
        _world.OnRoomExit += DisableRoomContents;
        _world.OnRoomEnter += EnableDoors;
        
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

            GameObject doorGo = Instantiate(gen.DoorPrefab, center, rotation);

            Vector3 scale = doorGo.transform.localScale;
            scale.x = length;
            doorGo.transform.localScale = scale;

            _goDoors.Add(doorGo);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
