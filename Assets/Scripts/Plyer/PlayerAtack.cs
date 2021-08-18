using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAtack : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleAttack,_particleBoost;
    [SerializeField]
    private MatchSystem _matchSystem;
    [SerializeField]
    private LayerMask _layerMask;
    private PlayerMove _playerMove;
    [SerializeField]
    private PlayerLife _playerLife;

    [SerializeField]
    private float _radiusAtack, _timeBeforeAttack, _attackDamage, _attackDistence = 0.8f;
    private float _timeBeforeAttackBoost, _attackDamageBoost;
    private bool _isBoost;
    private IEnumerator _boostingCorotine;
    public float TimeBeforeAttack
    {
        get
        {
            return _isBoost ? _timeBeforeAttackBoost : _timeBeforeAttack;
        }
    }
    public float AttackDamage
    {
        get
        {
            return _isBoost ? _attackDamageBoost : _attackDamage;
        }
    }
    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        GameStageEvent.StartLevel += StartAtack;
    }
    private void StartAtack()
    {
        GameStageEvent.StartLevel -= StartAtack;

        StartCoroutine(Shooting());
    }
    private void Atack(Vector3 direction)
    {
        if (_particleAttack != null)
            _particleAttack.Play();

        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _radiusAtack, direction, out hit, _attackDistence - _radiusAtack, _layerMask))
        {
            _matchSystem.StartTryConsumeMatch(hit.collider.GetComponent<Chip>().Cell, AttackDamage);
        }
    }
    private IEnumerator Shooting()
    {
        while (GameStage.IsGameFlowe)
        {
            yield return new WaitForSeconds(TimeBeforeAttack);
            Atack(_playerMove.DirectionTravel);
        }
    }
    private IEnumerator Boosting(float time)
    {
        _isBoost = true;
        _particleBoost.Play();
        _playerLife.IsInvulnerability = true;
        yield return new WaitForSeconds(time);
        _playerLife.IsInvulnerability = false;
        _particleBoost.Stop();
        _isBoost = false;
    }
    public void Gain(float timeBoost, float increasedAttackPower, float increasedAttackSpeedt)
    {
        if (_boostingCorotine != null)
        {
            StopCoroutine(_boostingCorotine);
        }
        _boostingCorotine = Boosting(timeBoost);
        StartCoroutine(_boostingCorotine);
        _attackDamageBoost = increasedAttackPower;
        _timeBeforeAttackBoost = increasedAttackSpeedt;
    }
}
