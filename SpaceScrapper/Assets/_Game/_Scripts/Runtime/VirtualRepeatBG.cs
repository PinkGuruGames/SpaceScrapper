using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualRepeatBG : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector2 backgroundOffset;
    private Vector2 backgroundTileHalfSize;
    private Vector2 targetDistance;

    private void Awake()
    {
        backgroundTileHalfSize = new Vector2(50f, 50f);
    }

    private void Update()
    {
        MoveBackground();
    }

    private void MoveBackground()
    {
        targetDistance = target.position - transform.position;
        if (targetDistance.x > backgroundTileHalfSize.x)
            backgroundOffset.x += backgroundTileHalfSize.x * 2;
        else if (targetDistance.x < -backgroundTileHalfSize.x)
            backgroundOffset.x -= backgroundTileHalfSize.x * 2;
        if (targetDistance.y > backgroundTileHalfSize.y)
            backgroundOffset.y += backgroundTileHalfSize.y * 2;
        else if (targetDistance.y < -backgroundTileHalfSize.y)
            backgroundOffset.y -= backgroundTileHalfSize.y * 2;
        if (backgroundOffset != Vector2.zero)
        {
            transform.position += (Vector3)backgroundOffset;
            backgroundOffset = Vector2.zero;
        }
    }
}
