using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Generation;
using Assets.Scripts.ScriptableObjects;
using Unity.VisualScripting;
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

                        // TODO: should be properly done
                        Enemy enemy = gameObject.GetComponent<Enemy>();
                        if (enemy != null)
                            room.AddEnemy(gameObject.GetComponent<Enemy>());
                        else 
                         room._decorationInstances.Add(gameObject);
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
            Enemy enemy = boss.GetComponent<Enemy>();
            if (enemy != null)
                room.AddEnemy(boss.GetComponent<Enemy>());
            else Debug.LogError("Boss game object does not contain an enemy component");
        }

        if (room.RoomType == RoomType.CHEST)
        {
            GameObject boss = Instantiate(
                gen.ChestPrefab,
                new Vector3(room.Rects[0].Center.x, 1, room.Rects[0].Center.y),
                Quaternion.Euler(0f, 180f, 0f)
            );
            room._decorationInstances.Add(boss);
        }

        // Si no se spawnean enemigos, que se quiten las puertas.
        if (room.EnemyCount <= 0)
        {
            room.AlreadyCleared = true;
        }

        room.HasSpawnedContents = true;
    }

    private void EnableRoomContents(Room room)
    {
        foreach (Enemy go in room._enemies)
        {
            if (go.gameObject != null)
                go.gameObject.SetActive(true);
        }
        foreach (GameObject go in room._decorationInstances)
        {
            if (go.gameObject != null)
                go.gameObject.SetActive(true);
        }
    }

    private void DisableRoomContents(Room room)
    {
        foreach (Enemy go in room._enemies)
        {
            if (go.gameObject != null)
                go.gameObject.SetActive(false);
        }

        foreach (GameObject go in room._decorationInstances)
        {
            if (go.gameObject != null)
                go.gameObject.SetActive(false);
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
        _world.OnRoomEnter += SpawnRoomContents;
        _world.OnRoomEnter += EnableRoomContents;
        _world.OnRoomExit += DisableRoomContents;

        _world.OnRoomEnter += StartEnableDoorsTimer;
        _world.OnRoomExit += CancelEnableDoorsTimer;
        _world.OnRoomCleared += OnRoomCleared;
    }

    private void OnRoomCleared(Room room)
    {
        SfxManager.Instance.PlaySfx("RoomCleared");

        if(room.RoomType == RoomType.BOSS)
        {
            UIManager.Instance.GameOverUI.ShowGameOver(true);
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

            Vector3 forward = rotation * Vector3.forward;
            center += forward * 1.0f;

            GameObject door_go = Instantiate(gen.DoorPrefab, center, rotation);

            Vector3 scale = door_go.transform.localScale;
            scale.x = length+1f;
            //scale.z = 0.1f;
            door_go.transform.localScale = scale;

            Room room = _world.GetRoomAtCell(new Vector2Int((int)door.Start.x, (int)door.Start.z));
            if (room == null)
            {
                room = _world.GetRoomAtCell(new Vector2Int((int)door.End.x, (int)door.End.z));
            }
            door_go.SetActive(false);
            room.AddDoor(door_go);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
