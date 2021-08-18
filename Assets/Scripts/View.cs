using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View : MonoBehaviour
{
    [SerializeField]
    private Vector3 _size = Vector3.one;

    [HideInInspector]
    [SerializeField]
    private bool _isActive;
    [ContextMenu("StateSwitch")]
    private void StateSwitch() => _isActive = !_isActive;
    private void OnDrawGizmos()
    {
        if (_isActive)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, _size);
        }
    }
}
