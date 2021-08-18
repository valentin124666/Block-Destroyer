using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _particleFinish;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerLife>() != null)
        {
            GameStage.Instance.ChangeStage(Stage.WinGame);
            _particleFinish.Play();
        }
    }
}
