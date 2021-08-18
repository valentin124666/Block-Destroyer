using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private Collider _coliderMain;
    [SerializeField]
    private Rigidbody _rbMain;

    public bool IsInvulnerability;
    private void OnTriggerEnter(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();

        if (chip != null)
        {
            if (chip.tag == "Bonus"||IsInvulnerability)
            {
                MatchSystem.Instance.StartTryConsumeMatch(chip.Cell,float.PositiveInfinity);
            }
            else
                Death();
        }

        if (other.gameObject.layer == 10)
        {
            Death();
        }
    }
    private void Death()
    {
        GameStage.Instance.ChangeStage(Stage.LostGame);
        _coliderMain.enabled = false;
        _rbMain.isKinematic = true;
        StartCoroutine(ActivationAnimation());
        enabled = false;
    }
    private IEnumerator ActivationAnimation()
    {
        _animator.SetBool("Death", true);
        yield return new WaitForSeconds(0.02f);
        _animator.SetBool("Death", false);
    }
}
