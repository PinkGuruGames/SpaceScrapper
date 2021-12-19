using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    public class FouPlayerController : MonoBehaviour
    {
        [SerializeField]
        private InputData inputData;

        [SerializeField]
        private float steerPower = 500f;

        [SerializeField]
        private float power = 5f;

        [SerializeField]
        private float maxSpeed = 10f;

        [SerializeField]
        [Tooltip("Maximum angle in degrees that thrusters can tilt to steer")]
        private float maxSteerAngle = 45f;

        [SerializeField]
        private Transform thrusters;
        [SerializeField]
        private Transform shipCenterMass;

        private Rigidbody rb;

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = shipCenterMass.position;
        }

        public void FixedUpdate()
        {
            //default direction
            var cachedTransform = transform;
            var cachedForward = cachedTransform.forward;
            var steer = inputData.Movement.x;
            var thrustersPosition = thrusters.position;

            if (steer != 0)
            {
                var steerThrustDirection =
                    Vector3.RotateTowards(cachedForward, -steer * cachedTransform.right, maxSteerAngle * Mathf.Deg2Rad,
                        0f);
                var steerForce = steerThrustDirection * steerPower;
                rb.AddForceAtPosition(steerThrustDirection * steerPower, thrustersPosition, ForceMode.Force);

                Debug.DrawRay(thrustersPosition, -steerForce, Color.red);
            }

            var forward = Vector3.Scale(new Vector3(1, 0, 1), cachedForward);

            PhysicsHelper.ApplyForceToReachVelocity(rb, forward * (maxSpeed * inputData.Movement.y), power);
        }
    }
}