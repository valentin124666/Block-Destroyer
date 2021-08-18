using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombChip : Chip
{
    [SerializeField]
    private int _explosionRadiusInCells;
    //[SerializeField]
    //private float _timeToExplosion;
    public override void StartConsume(float delay)
    {
        if (!_isConsume)
        {
            if (enabled)
                enabled = false;

            _isConsume = true;

            Detonation();
            StartCoroutine(Consume(0));
        }
    }
    private void Detonation()
    {
        int XMax = Cell.PosToGrid.x + _explosionRadiusInCells;
        int XMin = Cell.PosToGrid.x - _explosionRadiusInCells;

        int YMax = Cell.PosToGrid.y + _explosionRadiusInCells;
        int YMin = Cell.PosToGrid.y - _explosionRadiusInCells;

        List<Chip> chips = new List<Chip>();

        for (int x = XMin; x < XMax + 1; x++)
        {
            for (int y = YMin; y < YMax + 1; y++)
            {
                Cell cell = GridSystem.Instance.GetCell(new Vector2Int(x, y));

                if (cell?.Chip != null && cell.Chip != this)
                {
                    chips.Add(cell.Chip);
                }
            }
        }

        for (int i = 0; i < chips.Count; i++)
        {
            chips[i].StartConsume(0);
        }
    }

}
