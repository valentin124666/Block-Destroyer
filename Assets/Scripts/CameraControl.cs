using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private Transform _posPlayer;
    private Vector3 _target
    {
        get
        {
            if (_posPlayer == null)
            {
                PlayerMove player = FindObjectOfType<PlayerMove>();
                if (player == null)
                {
                    enabled = false;
                    return Vector3.zero;
                }
                else
                    _posPlayer = player.transform;
            }

            return new Vector3(transform.position.x, _posPlayer.position.y, transform.position.z);
        }
    }
    private Vector3 _velocity, _offSet;
    [SerializeField]
    private float _cameraSmoothness;
    void Start()
    {
        _offSet = _target - transform.position;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _target - _offSet, ref _velocity, _cameraSmoothness);
    }
}
