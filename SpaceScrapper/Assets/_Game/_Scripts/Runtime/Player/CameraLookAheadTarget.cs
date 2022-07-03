using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// Provides a Transform position relative to player and mouse position.
    /// </summary>
    public class CameraLookAheadTarget : MonoBehaviour
    {
        [SerializeField]
        private InputData input;
        [SerializeField]
        private Transform playerTransform;
        [SerializeField, Range(0,1)]
        private float lerp;
        [SerializeField]
        private float maxDistance = 6f;
        [SerializeField]
        private float minCollisionDistance;
        [SerializeField]
        LayerMask layerMask;

        private Vector2 velocity;

        // Update is called once per frame
        void FixedUpdate()
        {
            Vector2 center;
            center.x = Screen.width * 0.5f;
            center.y = Screen.height * 0.5f;

            var mousePos = input.ScreenMousePosition;

            var mouseOffset = mousePos - center;
            Vector2 normalizedOffset;
            normalizedOffset.x = mouseOffset.x / center.x;
            normalizedOffset.y = mouseOffset.y / center.y;

            Vector3 worldOffset = normalizedOffset * maxDistance;

            Vector2 targetPosition = playerTransform.position + worldOffset;
            Vector2 currentPosition = transform.position;
            Vector2 direction = targetPosition - (Vector2)playerTransform.position;
            float distance = direction.magnitude;
            direction /= distance;


            var hit = Physics2D.CircleCast(playerTransform.position, minCollisionDistance, direction, distance, layerMask);
            if(hit)
            {
                targetPosition = hit.centroid + hit.normal * 0.1f;
            }

            var nextPos = Vector2.SmoothDamp(currentPosition, targetPosition, ref velocity, lerp);

            transform.position = nextPos;

        }
    }
}
