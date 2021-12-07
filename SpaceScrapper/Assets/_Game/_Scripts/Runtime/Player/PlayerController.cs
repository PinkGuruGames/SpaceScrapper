using UnityEngine;

namespace SpaceScrapper
{
    public class PlayerController : MonoBehaviour
    {
        // References
        [SerializeField, HideInInspector] private new Rigidbody rigidbody;
        [SerializeField, HideInInspector] private Camera mainCamera;

        [Header("Options")]
        [SerializeField] private bool relativeMovement = false; // This could be later implemented as a setting, for now it's just for testing/prototyping purposes
        [SerializeField] private bool restrictTurnRate = false; // Whether the rotation rate should be limited or not. If it isn't, the ship produces some weird behaviour while the cursor is too close and shaken. 

        [Header("Movement Values")]
        [SerializeField] private float acceleration = 50;
        [SerializeField] private float topSpeed = 12;
        [SerializeField] private float turnSpeed = 50;
        [SerializeField] private float aimPrecision = 0.1f;

        private Vector2 collectiveInput;

        private void Update()
        {
            collectiveInput.x = Input.GetAxisRaw("Horizontal");
            collectiveInput.y = Input.GetAxisRaw("Vertical");
            collectiveInput = collectiveInput.normalized;
        }

        private void FixedUpdate()
        {
            Move();
            Aim();
        }

        private void Move()
        {
            float currentSqrSpeed = rigidbody.velocity.sqrMagnitude;

            if (collectiveInput.sqrMagnitude > 0.05f && currentSqrSpeed <= topSpeed * topSpeed)
            {
                if (relativeMovement)
                    rigidbody.AddRelativeForce(collectiveInput * acceleration);
                else
                    rigidbody.AddForce(collectiveInput * acceleration);
            }
            else if (currentSqrSpeed > 0.5f)
            {
                rigidbody.AddForce(-rigidbody.velocity.normalized * acceleration);
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
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
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
            xy.Raycast(ray, out var distance);
            return ray.GetPoint(distance);
        }
    }
}
