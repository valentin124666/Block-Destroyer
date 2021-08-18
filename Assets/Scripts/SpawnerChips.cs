using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerChips : MonoBehaviour
{
    [SerializeField]
    private Chip[] _chipsPrefans;

    public Chip SpawnChips(Transform Parent)
    {
        int namberChip = Random.Range(0,_chipsPrefans.Length);
        Chip chip = Instantiate(_chipsPrefans[namberChip], Parent.position, Parent.rotation);
        return chip;
    }
}
