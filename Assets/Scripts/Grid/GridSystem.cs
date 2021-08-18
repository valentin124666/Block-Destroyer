using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance;

    [SerializeField]
    private SpawnerChips _spawnerChips;
    [SerializeField]
    private Cell _cellPrefab;
    [SerializeField]
    private int _columnsCount, _rowsCount;
    [SerializeField]
    private float _cellWidth, _cellHeight;

    [SerializeField]
    private Cell[] _grid;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        SpawnChips();
    }

    [ContextMenu("Meshing")]
    private void Meshing()
    {
        if (_grid != null)
        {
            foreach (var item in _grid)
            {
                DestroyImmediate(item.gameObject);
            }
        }

        _grid = new Cell[_columnsCount * _rowsCount];

        for (int i = 0; i < _columnsCount; i++)
        {
            for (int j = 0; j < _rowsCount; j++)
            {
                Vector2Int postoGrid = new Vector2Int(i, j);
                Cell cell = Instantiate(_cellPrefab, PositionInGerid(postoGrid), Quaternion.identity);

                _grid[CalculateIndex(i, j)] = cell;
                cell.transform.SetParent(transform);
                cell.name = "Cell " + i + "x" + j;
            }
        }
    }
    private void SpawnChips()
    {
        for (int i = 0; i < _grid.Length; i++)
        {
            if (_grid[i].Chip==null)
            {
                Chip chip = _spawnerChips.SpawnChips(_grid[i].transform);
                _grid[i].Chip = chip;
            }
        }
    }
    public Cell GetCell(Vector2Int pos)
    {
        if (IsInBounds(pos))
        {
            return _grid[CalculateIndex(pos.x, pos.y)];
        }
        return null;
    }
    private int CalculateIndex(int x, int y)
    {
        return y * _columnsCount + x;
    }
    private Vector3 PositionInGerid(Vector2 gridPosition)
    {
        Vector3 localWorldPos = new Vector3(
                gridPosition.x * _cellWidth,
                gridPosition.y * _cellHeight,
                0
            );
        return localWorldPos + transform.position;
    }
    private bool IsInBounds(Vector2Int pos)
    {
        return pos.x >= 0 &&
               pos.x < _columnsCount &&
               pos.y >= 0 &&
               pos.y < _rowsCount;
    }
    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return PositionInGerid(gridPosition);
    }
    public Vector2Int WorldToGrid(Vector3 position)
    {
        Vector3 localWorldPos = position - transform.position;
        Vector2Int gridPos = new Vector2Int(
                (int)(localWorldPos.x / _cellWidth),
                (int)(localWorldPos.y / _cellHeight)
            );
        return gridPos;
    }
    public void UpdateStateCell()
    {
        foreach (var item in _grid)
        {
            if (item.Chip != null)
            {
                item.Chip.СheckingСhanges();
            }
        }
    }
}
