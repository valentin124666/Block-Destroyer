using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelPlayer : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    private Quaternion _rotationModel;
    [SerializeField]
    private float _speedRotationModel, _speedRotationDrill;
    private void Start()
    {
        StandStraight();
    }
    private void FixedUpdate()
    {
        _animator.SetBool("Start",GameStage.IsGameFlowe);
        transform.localRotation = Quaternion.Slerp(transform.localRotation,_rotationModel,_speedRotationModel);
    }

    public void StandStraight()
    {
        _animator.SetBool("Turn", false);
        _rotationModel = Quaternion.Euler(new Vector3(0, 180, 0));
    }
    public void TurnSideways(int side)
    {
        _animator.SetBool("Turn",true);
        _rotationModel = Quaternion.Euler(new Vector3(0, 90*side, 0));
    }
}
