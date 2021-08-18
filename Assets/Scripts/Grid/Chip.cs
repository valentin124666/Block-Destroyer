using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chip : MonoBehaviour
{
    private Cell _cell;
    [SerializeField]
    private ParticleSystem _destroyParticle;
    [SerializeField]
    private MoveChip _moveChip;
    [SerializeField]
    private Animator _animator;
    private Chip[] _chipsGroup;
    private IEnumerator _consumeCorotine;

    [SerializeField]
    private float _hangTimer;
    private float _hangTime;
    [SerializeField]
    private int _colorId;
    private float _delayDeath;
    //[SerializeField]
    protected bool _isConsume;

    public float Health;
    public int ColorId => _colorId;
    public bool IsSteadiness { get; private set; }

    public Cell Cell
    {
        get
        {
            if (_cell == null)
            {
                _cell = transform.GetComponentInParent<Cell>();
            }
            return _cell;
        }
    }

    void Start()
    {
        _chipsGroup = MatchSystem.Instance.GetGroup(Cell);
        IsSteadiness = true;
        _hangTime = _hangTimer;
        _moveChip.FinishedTheWay += СheckingСhanges;
        _moveChip.FinishedTheWay += Atack;
    }
    void FixedUpdate()
    {
        if (!_moveChip.enabled && !CommitCheck())
        {
            _animator.SetBool("Rattling", true);

            //MatchSystem.Instance.GroupStabilityCheck(Cell);
            if (_hangTime < 0)
            {
                _animator.SetBool("Rattling", false);

                Cell cell = GridSystem.Instance.GetCell(Cell.PosToGrid + Vector2Int.down);
                if (cell.Chip == null)
                {
                    _moveChip.ActivationMove(cell.transform);
                    Cell.Chip = null;
                    _cell = cell;

                    Cell.Chip = this;
                }
                else
                {
                    СheckingСhanges();
                }
            }
            else
            {
                _hangTime -= Time.deltaTime;
            }
        }
        else
        {
            _animator.SetBool("Rattling", false);
        }
    }
    public virtual void StartConsume(float delay)
    {
        if (!_isConsume)
        {
            enabled = false;
            _delayDeath = delay;
            _consumeCorotine = Consume(delay);
            StartCoroutine(_consumeCorotine);
        }
        else
        {
            if (_delayDeath>delay)
            {
                StopCoroutine(_consumeCorotine);
                _delayDeath = delay;
                _consumeCorotine = Consume(delay);
                StartCoroutine(_consumeCorotine);
            }
        }
    }
    protected IEnumerator Consume(float delay)
    {
        _animator.SetBool("Rattling", false);
        _animator.SetBool("Blink", true);

        _isConsume = true;
        yield return new WaitForSeconds(delay);
        if (_destroyParticle != null)
        {
            ParticleSystem particle = Instantiate(_destroyParticle, Cell.transform.position, Cell.transform.rotation);
            particle.Play();
        }

        Cell.Chip = null;
        Destroy(gameObject);
    }
    private void CommitFloor()
    {
        Cell cellDown = GridSystem.Instance.GetCell(Cell.PosToGrid + Vector2Int.down);
        if (cellDown == null || (cellDown.Chip != null && cellDown.Chip.IsSteadiness))
        {
            IsSteadiness = true;
        }
        else
        {
            IsSteadiness = false;
        }
    }
    private bool CommitCheck()
    {
        if (IsSteadiness || CommitCheckGroup())
        {
            _hangTime = _hangTimer;
            return true;
        }
        else
            return false;
    }
    private bool CommitCheckGroup()
    {
        if (_chipsGroup == null) return false;

        foreach (var item in _chipsGroup)
        {
            if (item.IsSteadiness) return true;
        }

        return false;
    }
    private void Atack()
    {
        if (CommitCheck())
        {
            MatchSystem.Instance.StartTryConsumeMatch(Cell);
        }
    }
    public void СheckingСhanges()
    {
        CommitFloor();
        Chip[] grop = MatchSystem.Instance.GetGroup(Cell);
        //if (grop.Length>_chipsGroup.Length)
        //{
        //    _hangTime = _hangTimer;
        //}
        _chipsGroup = grop;
    }
}
