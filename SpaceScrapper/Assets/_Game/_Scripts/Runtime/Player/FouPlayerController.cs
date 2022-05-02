using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class FouPlayerController : MonoBehaviour
    {
        [SerializeField]
        private InputData inputData;

        [SerializeField]
        private float steerPower = 2f;

        [SerializeField]
        private float power = 5f;

        [SerializeField]
        private float maxSpeed = 20f;

        [SerializeField]
        [Tooltip("Maximum angle in degrees that thrusters can tilt to steer")]
        private float maxSteerAngle = 45f;

        [SerializeField]
        private Transform thrusters;
        [SerializeField]
        private Transform shipCenterMass;

        private Rigidbody2D rb;

        public void Awake()
        {
            //set up rigidbody.
            rb = GetComponent<Rigidbody2D>();
            rb.centerOfMass = shipCenterMass.localPosition; //should always use localposition (as in relative to the rigidbody transform), never worldspace.
        }

        public void FixedUpdate()
        {
            //default direction
            var cachedTransform = transform;
            var cachedForward = cachedTransform.up;
            var steer = inputData.Movement.x;
            var thrustersPosition = thrusters.position;

            if (steer != 0)
            {
                var steerThrustDirection =
                    Vector3.RotateTowards(cachedForward, -steer * cachedTransform.right, maxSteerAngle * Mathf.Deg2Rad,
                        0f);
                var steerForce = steerThrustDirection * steerPower;
                rb.AddForceAtPosition(steerThrustDirection * steerPower, thrustersPosition, ForceMode2D.Force);

                Debug.DrawRay(thrustersPosition, -steerForce, Color.red);
            }

            var forward = Vector2.Scale(new Vector2(1, 1), cachedForward);

            PhysicsHelper.ApplyForceToReachVelocity(rb, forward * (maxSpeed * inputData.Movement.y), power);
        }
    }
}