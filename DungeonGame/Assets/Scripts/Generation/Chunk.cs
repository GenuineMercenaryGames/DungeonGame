using Assets.Scripts.ScriptableObjects;
using UnityEngine;

namespace Assets.Scripts.Generation
{
    public class Chunk
    {
        private World _world;
        private Vector3 _worldPosition;

        private Grid2D<CellType> _grid;

        // TODO: Change approach. This needs further talk as it depends
        // on how assets will be done, etc.
        private DungeonGenerator _currentGenerator;
        private Matrix4x4[] _treeMatrices;
        private int _treeCount;

        private bool _isPopulated;

        public Chunk(Vector3 worldPosition, int chunkCellSize, World world)
        {
            _world = world;
            _grid = new Grid2D<CellType>(new Vector2Int(chunkCellSize, chunkCellSize), Vector2Int.zero);
            _worldPosition = worldPosition;
            
            // Tree stuff
            // TODO: Generalize (with a better approach) to handle different types of obstacles
            _treeMatrices = new Matrix4x4[chunkCellSize * chunkCellSize];
            _treeCount = 0;

            _isPopulated = false;
        }

        public void PopulateChunk(DungeonGenerator generator)
        {
            _currentGenerator = generator;
            for (int x = 0; x < _grid.Size.x; x++) 
            {
                for (int y = 0; y < _grid.Size.y; y++)
                {
                    Vector2Int pos = new Vector2Int(x, y);
                    if (_grid[pos] == CellType.NONE)
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
        }

        public void RenderChunk()
        {
            if (!_isPopulated) return;

            Vector3 size = new Vector3(_grid.Size.x, 0.25f, _grid.Size.y);
            Vector3 position = _worldPosition + new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f);
            Matrix4x4 floorMatrix = Matrix4x4.TRS(position, Quaternion.identity, size);
            Graphics.RenderMesh(in _currentGenerator.RParamsFloor, _currentGenerator.floorPlane, 0, floorMatrix);

            if (_treeCount == 0) return;
            Graphics.RenderMeshInstanced(in _currentGenerator.RParamsTrees, _currentGenerator.TreeMesh, 0, _treeMatrices, _treeCount, 0);
        }

        public void SetCell(Vector2Int pos, CellType cell)
        {
            _grid[pos] = cell;
        }

        public CellType GetCell(Vector2Int pos)
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
                        case CellType.NONE:
                            Gizmos.color = Color.red;
                            break;
                        case CellType.HALLWAY:
                            Gizmos.color = Color.blue;
                            break;
                        case CellType.ROOM:
                            Gizmos.color = Color.purple;
                            break;
                    }

                    Vector3 pos = new Vector3(x, 0.0f, y) + _worldPosition;
                    Gizmos.DrawCube(pos - new Vector3(0.5f, 0.0f, 0.5f), Vector3.one);
                }
            }
        }
    }
}