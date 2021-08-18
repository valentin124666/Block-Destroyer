using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public delegate void CellChip(Cell cell, Chip chip);
    public event CellChip OnChipChanged;
    public Vector2Int PosToGrid { get; private set; }
    [SerializeField]
    private Chip _chipManualSpawn;
    [HideInInspector]
    [SerializeField]
    private Chip _chip;
    public Chip Chip
    {
        get { return _chip; }
        set
        {
            _chip = value;

            if (_chip != null)
            {
                _chip.transform.SetParent(transform);
            }
        }
    }

    private void Start()
    {
        PosToGrid = GridSystem.Instance.WorldToGrid(transform.position);
    }
    [ContextMenu("Chip Manual Spawn")]
    private void ChipManualSpawn()
    {
        if (_chipManualSpawn != null)
        {
            if (Chip != null)
                DestroyImmediate(Chip.gameObject);

            Chip chip = Instantiate(_chipManualSpawn, transform.position, transform.rotation);
            Chip = chip;
        }
    }
    [ContextMenu("Chip Manual Destroy")]
    private void ChipManualDestroy()
    {
        if (Chip != null)
            DestroyImmediate(Chip.gameObject);
    }
}
