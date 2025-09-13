using System;
using System.Linq;
using System.Collections.Generic;
using CORE;
using UnityEngine;

namespace BigEconomicGameJam
{
   

    public class BuildingService : AbstractMonoService, IInitializable
    {
        [SerializeField] private BaseEquipment[] _prefabs;
        [SerializeField] private float _gridSize = 2f;
        [SerializeField] private int _gridWidth = 10;
        [SerializeField] private int _gridHeight = 10;
        
        private bool[,] _buildGrid;
        private Dictionary<Vector2Int, IBuildingObject> _builtObjects = new Dictionary<Vector2Int, IBuildingObject>();
        private Vector3 _gridOrigin = Vector3.zero;

        public override Type RegisterType => typeof(BuildingService);

        public void Init()
        {
            InitializeBuildGrid();
        }
        
        private void InitializeBuildGrid()
        {
            _buildGrid = new bool[_gridWidth, _gridHeight];
            
            // Инициализируем всю сетку как доступную для строительства
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    _buildGrid[x, y] = true;
                }
            }
        }

        public IBuildingObject CreateObject(string id)
        {
            var prefab = _prefabs.First(x => x.ID == id);
            var instance = Instantiate(prefab);
            return instance;
        }

        public bool CanBuildAtPosition(Vector3 worldPosition)
        {
            Vector2Int gridPos = WorldToGridPosition(worldPosition);
            
            // Проверяем, находится ли позиция в пределах сетки
            if (!IsWithinGrid(gridPos))
                return false;
            
            // Проверяем, свободна ли ячейка
            return _buildGrid[gridPos.x, gridPos.y];
        }

        public void RegisterBuilding(IBuildingObject building, Vector3 worldPosition)
        {
            Vector2Int gridPos = WorldToGridPosition(worldPosition);
            
            if (!IsWithinGrid(gridPos))
            {
                Debug.LogError($"Cannot build outside grid boundaries at {gridPos}");
                return;
            }
            
            if (!_buildGrid[gridPos.x, gridPos.y])
            {
                Debug.LogError($"Grid cell {gridPos} is already occupied");
                return;
            }
            
            // Занимаем ячейку
            _buildGrid[gridPos.x, gridPos.y] = false;
            _builtObjects[gridPos] = building;
            
            // Устанавливаем точную позицию на сетке
            building.Position = GridToWorldPosition(gridPos);
            building.GridPosition = gridPos;
            
            // Вызываем событие постройки
            building.NotifyBuilt();
            
            Debug.Log($"Building registered at grid position: {gridPos}");
        }

        public void RemoveBuilding(IBuildingObject building)
        {
            if (_builtObjects.ContainsValue(building))
            {
                var entry = _builtObjects.FirstOrDefault(x => x.Value == building);
                if (entry.Value != null)
                {
                    _buildGrid[entry.Key.x, entry.Key.y] = true;
                    _builtObjects.Remove(entry.Key);
                    Debug.Log($"Building removed from grid position: {entry.Key}");
                }
            }
        }

        public void RemoveBuildingAt(Vector3 worldPosition)
        {
            Vector2Int gridPos = WorldToGridPosition(worldPosition);
            if (_builtObjects.ContainsKey(gridPos))
            {
                _buildGrid[gridPos.x, gridPos.y] = true;
                _builtObjects.Remove(gridPos);
                Debug.Log($"Building removed from grid position: {gridPos}");
            }
        }

        public bool IsCellOccupied(Vector2Int gridPosition)
        {
            if (!IsWithinGrid(gridPosition))
                return true; // Вне сетки считается занятым
            
            return !_buildGrid[gridPosition.x, gridPosition.y];
        }

        public Vector2Int WorldToGridPosition(Vector3 worldPosition)
        {
            int x = Mathf.RoundToInt((worldPosition.x - _gridOrigin.x) / _gridSize);
            int y = Mathf.RoundToInt((worldPosition.z - _gridOrigin.z) / _gridSize);
            
            return new Vector2Int(x, y);
        }

        public Vector3 GridToWorldPosition(Vector2Int gridPosition)
        {
            float x = _gridOrigin.x + gridPosition.x * _gridSize;
            float z = _gridOrigin.z + gridPosition.y * _gridSize;
            
            return new Vector3(x, 0, z);
        }

        public bool IsWithinGrid(Vector2Int gridPosition)
        {
            return gridPosition.x >= 0 && gridPosition.x < _gridWidth &&
                   gridPosition.y >= 0 && gridPosition.y < _gridHeight;
        }

        public Vector3 GetNearestBuildPosition(Vector3 worldPosition)
        {
            Vector2Int gridPos = WorldToGridPosition(worldPosition);
            
            // Ограничиваем позицию пределами сетки
            gridPos.x = Mathf.Clamp(gridPos.x, 0, _gridWidth - 1);
            gridPos.y = Mathf.Clamp(gridPos.y, 0, _gridHeight - 1);
            
            return GridToWorldPosition(gridPos);
        }

        // Визуализация сетки в редакторе (опционально)
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            
            for (int x = 0; x < _gridWidth; x++)
            {
                for (int y = 0; y < _gridHeight; y++)
                {
                    Vector3 center = GridToWorldPosition(new Vector2Int(x, y));
                    Gizmos.DrawWireCube(center + Vector3.up * 0.1f, new Vector3(_gridSize, 0.1f, _gridSize));
                }
            }
        }

        // Методы для отладки и управления сеткой
        public void ClearGrid()
        {
            InitializeBuildGrid();
            _builtObjects.Clear();
        }

        public void SetGridOrigin(Vector3 origin)
        {
            _gridOrigin = origin;
        }

        public Vector2Int GetGridSize()
        {
            return new Vector2Int(_gridWidth, _gridHeight);
        }

        public IEnumerable<IBuildingObject> GetAllBuildings()
        {
            return _builtObjects.Values;
        }
    }
}