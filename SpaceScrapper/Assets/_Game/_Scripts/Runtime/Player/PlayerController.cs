using UnityEngine;

namespace SpaceScrapper
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private InputData inputData;

        [Header("Movement Values")]
        [Tooltip("How fast the ship will accelerate when there is input from the player.")]
        [SerializeField] private float acceleration = 50f;
        [Tooltip("How fast the ship will stop when there is no input. Has no effect with auto breaking disabled.")]
        [SerializeField] private float deceleration = 50f;
        [Tooltip("How fast the ship will brake in cruise mode.")]
        [SerializeField] private float cruiseBrakeForce = 20f;
        [Tooltip("How fast the ship will turn in cruise mode.")]
        [SerializeField] private float cruiseTurnSpeed = 5f;
        [Tooltip("Speed until which the ship will accelerate.")]
        [SerializeField] private float topSpeed = 12f;
        [Tooltip("Minimum speed until which the auto breaks work.")]
        [SerializeField] private float minimumSpeed = 0f;
        [Tooltip("How fast the ship will turn towards the cursor.")]
        [SerializeField] private float turnSpeed = 50f;
        [Tooltip("TODO: This should be calculated from the turnSpeed and not be serialized")]
        [SerializeField] private float aimPrecision = 0.1f; // TODO: This should be calculated from the turnSpeed

        private void Update()
        {
            // TODO: Create coroutine that'd swap between cruise/battle mode
            // During swap, no change to velocity, rotate towards velocity.
        }

        private void FixedUpdate()
        {
            if (inputData.CruiseMode)
            {
                Cruise();
            }
            else
            {
                FreeMove();
                Aim();
            }
        }

        private void Cruise()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (inputData.Movement.y > 0.05f)
            {
                rigidbody.AddRelativeForce(Vector3.up * (inputData.Movement.y * acceleration));
            }
            else if (inputData.Movement.y < -0.05f)
            {
                if (currentSqrSpeed > 0.5f)
                {
                    rigidbody.AddForce(-rigidbody.velocity.normalized * cruiseBrakeForce);
                }
                else
                {
                    rigidbody.velocity = Vector3.zero;
                }
            }
            // TODO: Fix cruise rotation controls
            var angle = inputData.Movement.x;
            rigidbody.angularVelocity = angle * cruiseTurnSpeed * Time.deltaTime * Vector3.forward;
        }

        private void FreeMove()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (inputData.Movement.sqrMagnitude > 0.05f)
            {
                if (inputData.RelativeMode)
                    rigidbody.AddRelativeForce(inputData.Movement * acceleration);
                else
                    rigidbody.AddForce(inputData.Movement * acceleration);
            }
            else if (inputData.FlightAssist)
            {
                if (minimumSpeed != 0f)
                {
                    if (currentSqrSpeed > minimumSpeed * minimumSpeed)
                    {
                        rigidbody.AddForce(-rigidbody.velocity.normalized * deceleration);
                    }
                }
                else
                {
                    if (currentSqrSpeed > 0.5f)
                    {
                        rigidbody.AddForce(-rigidbody.velocity.normalized * deceleration);
                    }
                    else
                    {
                        rigidbody.velocity = Vector3.zero;
                    }
                }
            }

            if (currentSqrSpeed >= topSpeed * topSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * topSpeed;
            }
        }

        private void Aim()
        {
            var aimDirection = inputData.WorldMousePosition - transform.position;
            var angle = Vector3.SignedAngle(transform.up, aimDirection, Vector3.forward);
            if (angle > aimPrecision || angle < -aimPrecision)
            {
                rigidbody.angularVelocity = angle * turnSpeed * Time.deltaTime * Vector3.forward;
            }
            else
            {
                rigidbody.angularVelocity = Vector3.zero;
            }
        }
    }
}
