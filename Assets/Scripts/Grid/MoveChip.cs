using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveChip : MonoBehaviour
{
    public delegate void Empty();
    public event Empty FinishedTheWay;
    private Transform _target;
    [SerializeField]
    private float _speed;
    void Awake()
    {
        Deactivation();
    }

    void FixedUpdate()
    {
        if ((_target.position-transform.position).magnitude<=0.2f)
        {
            transform.position = _target.position;
            Deactivation();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position,_target.position,_speed);
        }
    }
    public void ActivationMove(Transform target)
    {
        enabled = true;
        gameObject.layer = 9;
        _target = target;
    }
    private void Deactivation()
    {
        enabled = false;
        gameObject.layer = 8;
        FinishedTheWay?.Invoke();
    }
}
