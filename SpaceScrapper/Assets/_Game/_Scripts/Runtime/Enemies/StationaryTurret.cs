using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SpaceScrapper.Weapons;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SpaceScrapper
{
    /// <summary>
    /// A simple enemy type that does not move, and only rotates to track its target.
    /// </summary>
    [RequireComponent(typeof(SimpleEnemyEntity))]
    public class StationaryTurret : AIControllerBase
    {
        [Header("Stationary Turret Settings")]
        [SerializeField]
        private float minAngle;
        [SerializeField]
        private float maxAngle;
        [SerializeField, Tooltip("The speed in angles per second, with which to track the target.")]
        private float trackingSpeed;
        [SerializeField]
        private float maxDistance;
        [SerializeField]
        private Transform turretBase;
        [SerializeField]
        private AutomaticWeapon weapon;

        private float currentAngle;

        private bool isShooting = false;

        //Note: at some future point, we might want to consider taking the speed of the target into account when aiming
        protected override void Aim()
        {
            float nextAngle;
            if(Target)
            {
                if(isShooting is false)
                {
                    isShooting = true;
                    weapon.ToggleShooting();
                }
                //first step is to check if the target is within the specified angle and distance limit.
                Vector2 targetPos = Target.transform.position;
                Vector2 pos = transform.position;
                Vector2 offset = targetPos - pos;
                float distance = offset.magnitude;
                if(distance > maxDistance)
                {
                    Target = null;
                    return;
                }
                //check whether the angle to the target is within the bounds.
                Vector2 direction = offset / distance; //normalized offset
                float offAngle = Vector2.SignedAngle(transform.up, direction);
                if(offAngle < minAngle || offAngle > maxAngle)
                {
                    Target = null;
                    return;
                }
                //change rotation
                float deltaRotation = trackingSpeed * Time.deltaTime;
                //this ensures not overrotating for aim.
                nextAngle = Mathf.MoveTowards(currentAngle, offAngle, deltaRotation);
                //Clamp to ensure the general rotation limits
                nextAngle = Mathf.Clamp(nextAngle, minAngle, maxAngle);
            }
            else
            {
                if (isShooting is true)
                {
                    isShooting = false;
                    weapon.ToggleShooting();
                }
                //no target given, return to default rotation (0)
                nextAngle = Mathf.MoveTowards(currentAngle, 0, trackingSpeed * Time.deltaTime);
            }
            //apply the rotation if necessary --//i will always write "is false" instead of !condition, because its much more readable
            if(Mathf.Approximately(currentAngle, nextAngle) is false)
            {
                turretBase.localRotation = Quaternion.Euler(0, 0, currentAngle);
                currentAngle = nextAngle;
            }
        }

        protected override void Move()
        {
            //Leave empty. Stationary turrets dont move, because well, they're stationary.
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Vector3 position = transform.position;
            Vector3 startPoint = Quaternion.AngleAxis(minAngle, Vector3.forward) * transform.up;
            Vector3 endPoint = Quaternion.AngleAxis(maxAngle, Vector3.forward) * transform.up;
            Handles.color = Color.red;
            //the cone that defines the limits for aiming
            Handles.DrawWireArc(position, Vector3.forward, startPoint, maxAngle - minAngle, 1);
            Handles.DrawLine(position, position + endPoint);
            Handles.DrawLine(position, position + startPoint);
            //the aim direction
            Handles.DrawLine(position, position + turretBase.up * 1.5f);
            //"aggro range"
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(position, maxDistance);
        }
#endif
    }
}
