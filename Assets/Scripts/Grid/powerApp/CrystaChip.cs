using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystaChip : Chip
{
    [SerializeField]
    private int _namverOfCristall;
    public override void StartConsume(float delay)
    {
        if (!_isConsume)
        {
            enabled = false;
            FindObjectOfType<CanvasManager>().CrystalCollector(_namverOfCristall);
            StartCoroutine(Consume(0));
        }
    }
}
