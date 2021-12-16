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
        private float drag = 0.1f;

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
            var steer = -inputData.Movement.x;

            rb.AddForceAtPosition(cachedTransform.right * (steer * steerPower), thrusters.position, ForceMode.Force);

            var forward = Vector3.Scale(new Vector3(1, 0, 1), cachedForward);

            PhysicsHelper.ApplyForceToReachVelocity(rb, forward * (maxSpeed * inputData.Movement.y), power);

            // //moving forward
            // var movingForward = Vector3.Cross(cachedForward, rb.velocity).y < 0;
            //
            // //move in direction
            // rb.velocity =
            //     Quaternion.AngleAxis(
            //         Vector3.SignedAngle(rb.velocity, (movingForward ? 1f : 0f) * cachedForward, Vector3.up) *
            //         drag, Vector3.up) * rb.velocity;
        }
    }
}