using UnityEngine;

namespace SpaceScrapper
{
    public class PlayerController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private new Rigidbody rigidbody;
        [SerializeField] private Camera mainCamera;

        [Header("Options")]
        [SerializeField] private bool relativeMovement = false; // This could be later implemented as a setting, for now it's just for testing/prototyping purposes
        [SerializeField] private bool restrictTurnRate = true; // Whether the rotation rate should be limited or not. If it isn't, the ship produces some weird behaviour while the cursor is too close and shaken. 
        [SerializeField] private bool autoBraking = true; // Whether to have the auto breaking enabled, can be extended later for different flight assist mechanics
        [SerializeField] private bool freeMove = true; // Whether to have the auto breaking enabled, can be extended later for different flight assist mechanics

        [Header("Movement Values")]
        [SerializeField, Tooltip("How fast the ship will accelerate when there is input from the player.")] private float acceleration = 50f;
        [SerializeField, Tooltip("How fast the ship will stop when there is no input. Has no effect with auto breaking disabled.")] private float deceleration = 50f;
        [SerializeField, Tooltip("How fast the ship will brake in cruise mode.")] private float cruiseBrakeForce = 20f;
        [SerializeField, Tooltip("How fast the ship will turn in cruise mode.")] private float cruiseTurnSpeed = 5f;
        [SerializeField] private float topSpeed = 12f;
        [SerializeField, Tooltip("Minimum speed until which the auto breaks work.")] private float minimumSpeed = 0f;
        [SerializeField, Tooltip("How fast the ship will turn towards the cursor.")] private float turnSpeed = 50f;
        [SerializeField] private float aimPrecision = 0.1f; // TODO: This should be calculated from the turnSpeed

        private Vector2 collectiveInput;

        private void Update()
        {
            collectiveInput.x = Input.GetAxisRaw("Horizontal");
            collectiveInput.y = Input.GetAxisRaw("Vertical");
            collectiveInput = collectiveInput.normalized;

            if (Input.GetKeyDown(KeyCode.F))
            {
                autoBraking ^= true;
            }
            if(Input.GetKeyDown(KeyCode.Tab))
            {
                freeMove ^= true;
                restrictTurnRate ^= true;
                // TODO: Create coroutine that'd swap between cruise/battle mode
                // During swap, no change to velocity, rotate towards velocity.
            }
        }

        private void FixedUpdate()
        {
            if (freeMove)
            {
                FreeMove();
                Aim();
            }
            else
            {
                CruiseMove();
            }
        }

        private void CruiseMove()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (collectiveInput.y > 0.05f)
            {
                rigidbody.AddRelativeForce(Vector3.up * (collectiveInput.y * acceleration));
            }
            else if (collectiveInput.y < -0.05f)
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
            var angle = collectiveInput.x;
            rigidbody.angularVelocity = angle * cruiseTurnSpeed * Time.deltaTime * Vector3.forward;
        }

        private void FreeMove()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (collectiveInput.sqrMagnitude > 0.05f)
            {
                if (relativeMovement)
                    rigidbody.AddRelativeForce(collectiveInput * acceleration);
                else
                    rigidbody.AddForce(collectiveInput * acceleration);
            }
            else if (autoBraking)
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
            var aimDirection = GetWorldPositionOnPlane(Input.mousePosition, 0f) - transform.position;
            if (restrictTurnRate)
            {
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
            else
            {
                transform.up = aimDirection;
            }
        }

        public Vector3 GetWorldPositionOnPlane(Vector3 screenPosition, float z)
        {
            Ray ray = mainCamera.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
            xy.Raycast(ray, out var distance);
            return ray.GetPoint(distance);
        }
    }
}
