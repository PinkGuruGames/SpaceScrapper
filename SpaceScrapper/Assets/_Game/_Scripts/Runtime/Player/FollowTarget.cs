using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Transform target;

    Vector3 targetPosition;

    void LateUpdate()
    {
        targetPosition = target.position;
        targetPosition.z = transform.position.z;
        transform.position = targetPosition;
    }
}
