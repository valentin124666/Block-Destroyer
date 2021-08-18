using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Legs : MonoBehaviour
{
    [SerializeField]
    private List<Transform> _walls = new List<Transform>();

    private void OnTriggerEnter(Collider other)
    {
        if ((other.gameObject.layer == 8 && other.tag != "Bonus") || other.tag == "Wall")
        {
            _walls.Add(other.transform);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.layer == 8 && other.tag != "Bonus") || other.tag == "Wall")
        {
            _walls.Remove(other.transform);
        }
    }
    public Vector3 CheckDirection(Vector3 oldDirection)
    {
        foreach (var wall in _walls)
        {
            if (wall != null)
            {
                Vector3 ParentPosition = transform.parent.position;
                Vector3 target = wall.position;
                target.y = ParentPosition.y;
                Vector3 DirectionWall = (target - ParentPosition).normalized;
                if (Mathf.Round(DirectionWall.x) == Mathf.Round(oldDirection.normalized.x))
                {
                    oldDirection.x = 0;
                    break;
                }
            }
        }
        return oldDirection;
    }
}
