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
        [SerializeField] private bool autoBreaking = true; // Whether to have the auto breaking enabled, can be extended later for different flight assist mechanics

        [Header("Movement Values")]
        [SerializeField, Tooltip("How fast the ship will accelerate when there is input from the player.")] private float acceleration = 50;
        [SerializeField, Tooltip("How fast the ship will stop when there is no input. Has no effect with auto breaking disabled.")] private float deceleration = 50;
        [SerializeField] private float topSpeed = 12;
        [SerializeField, Tooltip("Minimum speed until which the auto breaks work.")] private float minimumSpeed = 1;
        [SerializeField, Tooltip("How fast the ship will turn towards the cursor.")] private float turnSpeed = 50;
        [SerializeField] private float aimPrecision = 0.1f; // TODO: This should be calculated from the turnSpeed

        private Vector2 collectiveInput;

        private void Update()
        {
            collectiveInput.x = Input.GetAxisRaw("Horizontal");
            collectiveInput.y = Input.GetAxisRaw("Vertical");
            collectiveInput = collectiveInput.normalized;

            if (Input.GetKeyDown(KeyCode.F))
            {
                autoBreaking ^= true;
            }
        }

        private void FixedUpdate()
        {
            Move();
            Aim();
        }

        private void Move()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (collectiveInput.sqrMagnitude > 0.05f)
            {
                if (relativeMovement)
                    rigidbody.AddRelativeForce(collectiveInput * acceleration);
                else
                    rigidbody.AddForce(collectiveInput * acceleration);
            }
            else if (autoBreaking)
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
