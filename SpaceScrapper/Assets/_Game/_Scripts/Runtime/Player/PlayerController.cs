using UnityEngine;

namespace SpaceScrapper
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private new Rigidbody2D rigidbody;
        [SerializeField] private InputData inputData;

        [Header("Hover Mode Movement Values")]
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

        [Header("Cruise Mode Movement Values")]
        [Tooltip("Speed until which the ship will accelerate in cruise mode.")]
        [SerializeField] private float cruiseAcceleration = 50f;
        [Tooltip("How fast the ship will brake in cruise mode.")]
        [SerializeField] private float cruiseDeceleration = 20f;
        [Tooltip("How fast the ship will turn in cruise mode.")]
        [SerializeField] private float cruiseTopSpeed = 12f;
        [Tooltip("How fast the ship will accelerate in cruise mode when there is input from the player.")]
        [SerializeField] private float cruiseTurnSpeed = 5f;
        [Tooltip("Lateral force to add in cruise mode to simulate atmospheric physics.")]
        [SerializeField] private float lateralCorrectionForce = 1f;
        [Tooltip("Maximum angle of attack allowed in cruise mode.")]
        [SerializeField] private float cruiseMaxAngleOfAttack = 5f;
        [Tooltip("How fast the ship should accelerate (X-Axis) in cruise mode in relation to its current speed (Y-Axis).")]
        [SerializeField] private AnimationCurve cruiseAccelerationCurve;

        private float hoverBreakPrecision;
        private float hoverAimPrecision;

        private void Start()
        {
            // Calculating via Time.fixedDeltaTime because how much we turn/break per frame shouldn't exceed the aim precision/break force, * 1.1f for a small safety net.
            hoverBreakPrecision = hoverDeceleration * Time.fixedDeltaTime * 1.1f;
            hoverAimPrecision = hoverTurnSpeed * Time.fixedDeltaTime * 1.1f;
        }

        private void FixedUpdate()
        {
            if (inputData.CruiseMode)
            {
                float accelerationMultiplier = cruiseAccelerationCurve.Evaluate(rigidbody.velocity.magnitude / cruiseTopSpeed);

                CruiseMove(accelerationMultiplier);
                CruiseAim(accelerationMultiplier);
            }
            else
            {
                FreeMove();
                FreeAim();
            }
        }

        private void CruiseMove(float accelerationMultiplier)
        {
            if (inputData.Movement.y > 0.05f)
            {
                rigidbody.AddRelativeForce(Vector3.up * (inputData.Movement.y * cruiseAcceleration * accelerationMultiplier));
            }
            else if (inputData.Movement.y < -0.05f)
            {
                if (rigidbody.velocity.sqrMagnitude > 0.5f)
                {
                    rigidbody.AddForce(-rigidbody.velocity.normalized * cruiseDeceleration);
                }
                else
                {
                    rigidbody.velocity = Vector3.zero;
                }
            }
        }

        private void CruiseAim(float accelerationMultiplier)    // TODO: Fix cruise rotation controls
        {
            var desiredDirection = inputData.Movement.x;
            var deltaRotation = desiredDirection * cruiseTurnSpeed * Time.deltaTime; // * Vector3.forward) ;
            rigidbody.MoveRotation(rigidbody.rotation + deltaRotation);

            var angle = Vector3.SignedAngle(transform.up, rigidbody.velocity, Vector3.forward);
            var lateralForceToAdd = accelerationMultiplier * lateralCorrectionForce;
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
                    if (currentSqrSpeed > hoverBreakPrecision * hoverBreakPrecision)
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

        private void FreeAim()
        {
            var aimDirection = inputData.WorldMousePosition - transform.position;
            var angle = Vector2.SignedAngle(transform.up, aimDirection);

            if (angle > hoverAimPrecision || angle < -hoverAimPrecision)
            {
                var direction = angle < 0 ? -1 : 1; // TODO: Optimize

                var deltaRotation = direction * hoverTurnSpeed * Time.deltaTime;
                rigidbody.MoveRotation(rigidbody.rotation + deltaRotation);
            }
            else
            {
                rigidbody.angularVelocity = 0;
                transform.up = aimDirection;
            }
        }
    }
}
