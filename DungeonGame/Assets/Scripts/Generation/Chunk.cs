using Assets.Scripts.ScriptableObjects;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Generation
{
    public class Chunk
    {
        public bool IsLoaded { get; private set; }

        private World _world;
        private Vector3 _worldPosition;

        private Grid2D<ushort> _grid;

        // TODO: Change approach. This needs further talk as it depends
        // on how assets will be done, etc.
        private DungeonGenerator _currentGenerator;
        private Matrix4x4[] _treeMatrices;
        private int _treeCount;

        private bool _isPopulated;

        public Mesh WalkablePlane;

        public Chunk(Vector3 worldPosition, int chunkCellSize, World world)
        {
            _world = world;
            _grid = new Grid2D<ushort>(new Vector2Int(chunkCellSize, chunkCellSize), Vector2Int.zero);
            _worldPosition = worldPosition;
            
            // Tree stuff
            // TODO: Generalize (with a better approach) to handle different types of obstacles
            _treeMatrices = new Matrix4x4[chunkCellSize * chunkCellSize];
            _treeCount = 0;

            _isPopulated = false;

            WalkablePlane = new Mesh();
        }

        public void PopulateChunk(DungeonGenerator generator)
        {
            _currentGenerator = generator;
            for (int x = 0; x < _grid.Size.x; x++) 
            {
                for (int y = 0; y < _grid.Size.y; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (_grid[pos] == World.CELL_TYPE_EMPTY)
                    {
                        // Check if tree can be generated
                        float r = _world.GetRandom().Next(0, 100);
                        if(r < generator.TreeDensity)
                        {
                            // Add tree
                            Vector3 offset = new Vector3(_world.GetRandom().Next(0, 50) / 100.0f, 0.0f, _world.GetRandom().Next(0, 50) / 100.0f);
                            Matrix4x4 m = Matrix4x4.TRS(new Vector3(x, 0.0f, y) + offset + _worldPosition,
                                Quaternion.identity, 
                                Vector3.one);
                            _treeMatrices[_treeCount++] = m;
                        }
                    }
                }
            }

            _isPopulated = true;


            // Create walkable mesh
            // TODO: Remake properly
            List<Vector3> vertices = new List<Vector3>();
            List<int> indices = new List<int>();
            for (int x = 0; x < _grid.Size.x; x++)
            {
                for (int y = 0; y < _grid.Size.y; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (_grid[pos] >= World.CELL_TYPE_ROOM || _grid[pos] == World.CELL_TYPE_HALLWAY)
                    {
                        float xp = x + _worldPosition.x;
                        float yp = _worldPosition.y;
                        float zp = y + _worldPosition.z;
                        int v = vertices.Count;
                        vertices.Add(new Vector3(xp, yp, zp));
                        vertices.Add(new Vector3(xp + 1.0f, yp, zp));
                        vertices.Add(new Vector3(xp, yp, zp + 1.0f));
                        vertices.Add(new Vector3(xp + 1.0f, yp, zp + 1.0f));

                        indices.Add(v);
                        indices.Add(v + 3);
                        indices.Add(v + 1);
                        indices.Add(v);
                        indices.Add(v + 2);
                        indices.Add(v + 3);
                    }
                }
            }

            WalkablePlane.SetVertices(vertices);
            WalkablePlane.SetIndices(indices, MeshTopology.Triangles, 0);
            WalkablePlane.RecalculateNormals();


            // Spawn stuff
            // TODO: REMAKE PROPERLY
            /* Commented temporarily? If spawn is handled elsewhere in the end, then remove this code
            for (int x = 0; x < _grid.Size.x; x++)
            {
                for (int y = 0; y < _grid.Size.y; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (_grid[pos] == CellType.ROOM || _grid[pos] == CellType.HALLWAY)
                    {
                        // Te he metido esta comprobación para evitar que pete sin prefabs - kike
                        if (_currentGenerator.PrefabsToSpawn.Length != 0) 
                        {
                            int p = _world.GetRandom().Next(0, _currentGenerator.PrefabsToSpawn.Length);
                            float r = _world.GetRandom().Next(0, 100) / 100.0f;
                            if (r <= _currentGenerator.PrefabsProbabilities[p])
                            {
                                GameObject gameObject = GameObject.Instantiate(_currentGenerator.PrefabsToSpawn[p], new Vector3(pos.x, 0.0f, pos.y) + _worldPosition, Quaternion.identity);
                            }
                        }
                    }
                }
            }*/
        }

        public void LoadChunk()
        {
            IsLoaded = true;
        }

        public void UnloadChunk()
        {
            IsLoaded = false;
        }

        public void RenderChunk()
        {
            if (!_isPopulated) return;

            Vector3 size = new Vector3(_grid.Size.x, 0.25f, _grid.Size.y);
            Vector3 position = Vector3.zero;// _worldPosition;// + new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);
            Matrix4x4 floorMatrix = Matrix4x4.TRS(position, Quaternion.identity, Vector3.one);
            Graphics.RenderMesh(in _currentGenerator.RParamsFloor, WalkablePlane, 0, floorMatrix);

            if (_treeCount == 0) return;
            Graphics.RenderMeshInstanced(in _currentGenerator.RParamsTrees, _currentGenerator.TreeMesh, 0, _treeMatrices, _treeCount, 0);
        }

        public void SetCell(Vector2Int pos, ushort cell)
        {
            _grid[pos] = cell;
        }

        public ushort GetCell(Vector2Int pos)
        {
            return _grid[pos];
        }

        public void DrawGizmos()
        {
            for(int x = 0; x < _grid.Size.x; x++)
            {
                for (int y = 0; y < _grid.Size.y; y++)
                {
                    switch(_grid[new Vector2Int(x, y)])
                    {
                        case World.CELL_TYPE_EMPTY:
                            Gizmos.color = Color.red;
                            break;
                        case World.CELL_TYPE_HALLWAY:
                            Gizmos.color = Color.blue;
                            break;
                        case World.CELL_TYPE_ROOM:
                        default:
                            Gizmos.color = Color.purple;
                            break;
                    }

                    Vector3 pos = new Vector3(x, 0.0f, y) + _worldPosition;
                    Gizmos.DrawSphere(pos - new Vector3(0.5f, 0.0f, 0.5f), 0.5f);
                }
            }
        }
    }
}