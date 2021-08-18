using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public static Transform Transform { get; private set; }

    [SerializeField]
    private ModelPlayer _modelPlayer;
    private Vector3 _startTouchPos, _currentPosPlayer, _targetPosPlayer;
    private Vector3 _directionTravel; public Vector3 DirectionTravel
    {
        get { return _directionTravel; }
        private set
        {
            if (value.x > 0)
            {
                _directionTravel = Vector3.right;
            }
            else if (value.x < 0)
            {
                _directionTravel = Vector3.left;
            }
            else
            {
                _directionTravel = Vector3.down;
            }

        }
    }
    private Vector3 _startPos;
    private Camera _cam;
    [SerializeField]
    private Rigidbody _rbMain;
    [SerializeField]
    private Legs _legs;

    [SerializeField]
    private float _speed, _Offset;
    [SerializeField]
    private bool _isTravel;
    private float _widthFarPoint
    { get { return _startPos.x + _Offset; } }
    private float _widthNearPoint
    { get { return _startPos.x - _Offset; } }

    private void Awake()
    {
        _startPos = transform.position;
        Transform = transform;
    }

    void Start()
    {
        _targetPosPlayer = transform.position;
        _cam = Camera.main;
    }
    private void Update()
    {
        if (GameStage.IsGameFlowe)
        {
            if (TouchUtility.TouchCount > 0)
            {
                Touch touch = TouchUtility.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                    _currentPosPlayer = transform.position;

                    _startTouchPos = (_cam.transform.position - ((ray.direction) *
                            ((_cam.transform.position - transform.position).z / ray.direction.z)));
                }
                else if (touch.phase == TouchPhase.Moved)
                {
                    Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

                    if (_startTouchPos == Vector3.zero)
                    {
                        _currentPosPlayer = transform.position;

                        _startTouchPos = (_cam.transform.position - ((ray.direction) *
                                ((_cam.transform.position - transform.position).z / ray.direction.z)));
                    }


                    _targetPosPlayer = (_currentPosPlayer + ((_cam.transform.position - ((ray.direction) *
                            ((_cam.transform.position - transform.position).z / ray.direction.z))) - _startTouchPos));
                }
            }
            else
            {
                _targetPosPlayer = transform.position;
            }
        }
    }
    void FixedUpdate()
    {
        Travel();
    }
    private float GetPositionAbscissa(float X)
    {
        if (_widthFarPoint < X)
        {
            return _widthFarPoint;
        }
        else if (_widthNearPoint > X)
        {
            return _widthNearPoint;
        }
        else
        {
            return X;
        }
    }
    private void Travel()
    {
        Vector3 target = transform.position;
        if (Mathf.Abs(_targetPosPlayer.x - transform.position.x) > 0.3f)
            _isTravel = true;
        else if (Mathf.Abs(_targetPosPlayer.x - transform.position.x) < 0.04f)
            _isTravel = false;

        if (_isTravel)
        {
            target.x = GetPositionAbscissa(_targetPosPlayer.x);
        }

        Vector3 direction = (target - transform.position).normalized * _speed;
        direction.y = _rbMain.velocity.y;
        _rbMain.velocity = _legs.CheckDirection(direction);

        DirectionTravel = direction;

        if (_isTravel)
            RotationModel(DirectionTravel.x);
        else
            RotationModel(0);
    }
    private void RotationModel(float x)
    {
        if (x != 0)
        {
            int side = x > 0 ? 1 : -1;
            _modelPlayer.TurnSideways(side);
        }
        else
        {
            _modelPlayer.StandStraight();
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.grey;
        Vector3 from = transform.position;
        Vector3 to = transform.position;
        from.x = transform.position.x - _Offset;
        to.x = transform.position.x + _Offset;
        Gizmos.DrawLine(from, to);
    }

}
