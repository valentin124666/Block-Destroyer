using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchSystem : MonoBehaviour
{
    public static MatchSystem Instance;
    [SerializeField]
    private int _minAmountForDestruction;
    [SerializeField]
    private float _delayBeforeDestruction, _delayBeforeDestructionPlayer;
    private void Awake()
    {
        Instance = this;
    }
    public Chip[] GetGroup(Cell originCell)
    {
        Queue<Cell> cellsToProcess = new Queue<Cell>();
        HashSet<Cell> processedCells = new HashSet<Cell>();
        List<Chip> matchedChips = new List<Chip>();

        cellsToProcess.Enqueue(originCell);
        processedCells.Add(originCell);
        if (originCell.Chip.ColorId > 0)
        {
            while (cellsToProcess.Count > 0)
            {
                Cell cell = cellsToProcess.Dequeue();
                matchedChips.Add(cell.Chip);

                TryEnqueueNeighbour(cell, Vector2Int.left, ref cellsToProcess, ref processedCells);
                TryEnqueueNeighbour(cell, Vector2Int.right, ref cellsToProcess, ref processedCells);
                TryEnqueueNeighbour(cell, Vector2Int.up, ref cellsToProcess, ref processedCells);
                TryEnqueueNeighbour(cell, Vector2Int.down, ref cellsToProcess, ref processedCells);
            }
        }
        else
        {
            matchedChips.Add(originCell.Chip);
        }

        return matchedChips.ToArray();
    }
    private void TryEnqueueNeighbour(Cell cell, Vector2Int neighbourOffset, ref Queue<Cell> cellsToProcess, ref HashSet<Cell> processedCells)
    {
        Cell neighbour = GridSystem.Instance.GetCell(cell.PosToGrid + neighbourOffset);
        if (!processedCells.Contains(neighbour) && neighbour?.Chip != null && cell.Chip.ColorId == neighbour.Chip.ColorId)
        {
            processedCells.Add(neighbour);
            cellsToProcess.Enqueue(neighbour);
        }
    }
    private IEnumerator TryConsumeMatch(Cell originCell, float damage)
    {
        originCell.Chip.Health -= damage;
        if (float.IsNaN(originCell.Chip.Health)) originCell.Chip.Health = 0;

        if (originCell.Chip.Health <= 0)
        {

            Chip[] chips = GetGroup(originCell);

            foreach (Chip chip in chips)
            {
                chip.StartConsume(_delayBeforeDestructionPlayer);
            }
            yield return new WaitForSeconds(_delayBeforeDestructionPlayer);
            GridSystem.Instance.UpdateStateCell();
        }

    }
    private IEnumerator TryConsumeMatch(Cell originCell)
    {
        Chip[] chips = GetGroup(originCell);
        if (chips.Length >= _minAmountForDestruction)
        {
            foreach (Chip chip in chips)
            {
                chip.StartConsume(_delayBeforeDestruction);
            }
        }
        yield return new WaitForSeconds(_delayBeforeDestruction);

        GridSystem.Instance.UpdateStateCell();

    }
    public void StartTryConsumeMatch(Cell originCell, float damage)
    {
        if (originCell?.Chip != null)
            StartCoroutine(TryConsumeMatch(originCell, damage));
    }
    public void StartTryConsumeMatch(Cell originCell)
    {
        if (originCell?.Chip != null)
            StartCoroutine(TryConsumeMatch(originCell));
    }
}
