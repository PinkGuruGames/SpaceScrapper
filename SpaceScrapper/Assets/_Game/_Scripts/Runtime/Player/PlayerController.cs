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
        [SerializeField] private float hoverAcceleration = 50f;
        [Tooltip("How fast the ship will stop when there is no input. Has no effect with auto breaking disabled.")]
        [SerializeField] private float hoverDeceleration = 50f;
        [Tooltip("Speed until which the ship will accelerate.")]
        [SerializeField] private float hoverTopSpeed = 100f;
        [Tooltip("Minimum speed until which the auto breaks work.")]
        [SerializeField] private float hoverMinimumSpeed = 0f;
        [Tooltip("How fast the ship will turn towards the cursor. Measuerd in degrees per second.")]
        [SerializeField] private float hoverTurnSpeed = 50f;
        [Tooltip("Speed until which the ship will accelerate in cruise mode.")]
        [SerializeField] private float cruiseTopSpeed = 12f;
        [Tooltip("How fast the ship will brake in cruise mode.")]
        [SerializeField] private float cruiseBrakeForce = 20f;
        [Tooltip("How fast the ship will turn in cruise mode.")]
        [SerializeField] private float cruiseTurnSpeed = 5f;
        [Tooltip("Lateral force to add.")]
        [SerializeField] private float lateralCorrectionForce = 1f;
        [Tooltip("Maximum angle of attack allowed before force is applied towards rotation.")]
        [SerializeField] private float cruiseMaxAngleOfAttack = 5f;
        [Tooltip("How fast the ship should accelerate (X-Axis) in cruise mode in relation to its current speed (Y-Axis).")]
        [SerializeField] private AnimationCurve cruiseAcceleration;

        private float aimPrecision = 0.1f;

        private void Update()
        {
            // This should be in Start() after testing is done, it's currently in update to allow updating hoverTurnSpeed in runtime to test;
            aimPrecision = hoverTurnSpeed * Time.fixedDeltaTime * 1.1f;
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
            float accelerationCurve = cruiseAcceleration.Evaluate(Mathf.Sqrt(currentSqrSpeed) / cruiseTopSpeed); //

            if (inputData.Movement.y > 0.05f)
            {
                rigidbody.AddRelativeForce(Vector3.up * (inputData.Movement.y * hoverAcceleration * accelerationCurve));
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
            var desiredRotation = -inputData.Movement.x;
            rigidbody.angularVelocity = desiredRotation * cruiseTurnSpeed * Time.deltaTime * Vector3.forward;

            var angle = Vector2.SignedAngle(transform.up, rigidbody.velocity);
            var lateralForceToAdd = accelerationCurve * lateralCorrectionForce;
            if (angle > cruiseMaxAngleOfAttack || angle < -cruiseMaxAngleOfAttack)
            {
                rigidbody.AddRelativeForce(transform.right * (-angle * lateralForceToAdd));
            }
            rigidbody.AddRelativeForce(-rigidbody.velocity.normalized * (lateralForceToAdd * Mathf.Abs(angle)));
        }

        private void FreeMove()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (inputData.Movement.sqrMagnitude > 0.05f)
            {
                if (inputData.RelativeMode)
                    rigidbody.AddRelativeForce(inputData.Movement * hoverAcceleration);
                else
                    rigidbody.AddForce(inputData.Movement * hoverAcceleration);
            }
            else if (inputData.FlightAssist)
            {
                if (hoverMinimumSpeed != 0f)
                {
                    if (currentSqrSpeed > hoverMinimumSpeed * hoverMinimumSpeed)
                    {
                        rigidbody.AddForce(-rigidbody.velocity.normalized * hoverDeceleration);
                    }
                }
                else
                {
                    if (currentSqrSpeed > 0.5f)
                    {
                        rigidbody.AddForce(-rigidbody.velocity.normalized * hoverDeceleration);
                    }
                    else
                    {
                        rigidbody.velocity = Vector3.zero;
                    }
                }
            }

            if (currentSqrSpeed >= hoverTopSpeed * hoverTopSpeed)
            {
                rigidbody.velocity = rigidbody.velocity.normalized * hoverTopSpeed;
            }
        }

        private void Aim()
        {
            var aimDirection = inputData.WorldMousePosition - transform.position;
            var angle = Vector3.SignedAngle(transform.up, aimDirection, Vector3.forward);

            if (angle > aimPrecision || angle < -aimPrecision)
            {
                var direction = angle < 0 ? -1 : 1; // TODO: Optimize

                var deltaRotation = Quaternion.Euler(direction * hoverTurnSpeed * Time.deltaTime * Vector3.forward);
                rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
            }
            else
            {
                rigidbody.angularVelocity = Vector3.zero;
                transform.up = aimDirection;
            }
        }
    }
}
