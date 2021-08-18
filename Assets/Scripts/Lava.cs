using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lava : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    private void FixedUpdate()
    {
        if (GameStage.IsGameFlowe)
            transform.Translate(Vector3.down * _speed);
    }
    private void OnTriggerEnter(Collider other)
    {
        Chip chip = other.GetComponent<Chip>();
        if (chip != null && chip.enabled)
        {
            chip.StartConsume(0);
        }
    }
}
