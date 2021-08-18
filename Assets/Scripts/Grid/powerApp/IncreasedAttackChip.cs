using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasedAttackChip : Chip
{
    [SerializeField]
    private float _timeBoost, _increasedAttackPower, _increasedAttackSpeed;
    public override void StartConsume(float delay)
    {
        if (!_isConsume)
        {
            enabled = false;
            Boost();
            StartCoroutine(Consume(0));
        }
    }
    private void Boost()
    {
        FindObjectOfType<PlayerAtack>().Gain(_timeBoost,_increasedAttackPower,_increasedAttackSpeed);
    }
}
