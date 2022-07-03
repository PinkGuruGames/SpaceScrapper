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
        private SteerMode steerMode = SteerMode.Force;

        [SerializeField]
        private AnimationCurve steeringAlignmentCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField]
        private float steeringAlignmentTime = 1f;

        [SerializeField]
        private Transform thrusters;
        [SerializeField]
        private Transform shipCenterMass;

        private Rigidbody2D rb;

        private float lastSteeringTime;
        private float maxAngularVelocity;

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

            if (Mathf.Approximately(steer, 0) is false)
            {
                if (steerMode == SteerMode.Force)
                {
                    //old version:
                    //var steerThrustDirection =
                    //    Vector3.RotateTowards(cachedForward, -steer * cachedTransform.right, maxSteerAngle * Mathf.Deg2Rad,
                    //        0f);
                    //same, but with Quaternion rotating it. less clumsy imo.
                    Vector2 steerThrustDirection = Quaternion.AngleAxis(steer * maxSteerAngle, Vector3.forward) * transform.up;
                    //alternate version, just lateral, which shouldnt cause forward momentum.
                    //Vector2 steerThrustDirection = transform.right * -steer;
                    var steerForce = steerThrustDirection * steerPower;
                    rb.AddForceAtPosition(steerThrustDirection * steerPower, thrustersPosition, ForceMode2D.Force);

                    Debug.DrawRay(thrustersPosition, -steerForce, Color.red);
                }
                else
                {
                    //add torque based on steering direction
                    rb.AddTorque(-steer * 10f);
                    //update tracking params.
                    lastSteeringTime = Time.time;
                    maxAngularVelocity = rb.angularVelocity;
                }

            }
            else //no steering input.
            {
                //if no input turning input is given, automatically return to 0 rotation.
                if(steerMode == SteerMode.Torque && inputData.FlightAssist is true)
                {
                    float t = Time.time - lastSteeringTime;
                    //only correct it while during this specific timeframe.
                    if (t <= steeringAlignmentTime)
                    {
                        //dt = progress along the lerp (from 0 to steeringAlignmentTime => 0 to 1)
                        float dt = t / steeringAlignmentTime;
                        float dtOnCurve = steeringAlignmentCurve.Evaluate(dt);
                        //lerp the angular velocity down to 0, based on the animation curve.
                        float angularVelocity = Mathf.Lerp(maxAngularVelocity, 0, dtOnCurve);
                        rb.angularVelocity = angularVelocity;
                    }
                }
            }

            var forward = cachedForward; //Vector2.Scale(new Vector2(1, 1), cachedForward);

            PhysicsHelper.ApplyForceToReachVelocity(rb, forward * (maxSpeed * inputData.Movement.y), power);
        }

        public enum SteerMode
        {
            Force,
            Torque
        }
    }
}