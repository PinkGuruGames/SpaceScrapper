using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// Leans the model into the direction the body is moving in, to create a more dynamic look.
    /// </summary>
    //@author=Wokarol
    public class ModelLeaner : MonoBehaviour
    {
        [SerializeField]
        private new Rigidbody2D rigidbody;
        [SerializeField] 
        private Vector3 up = Vector3.back;
        [SerializeField] 
        private float maxVelocity = 10;
        [SerializeField] 
        private float maxLean = 30;
        [SerializeField, Range(0, 1)]
        private float minimumLeanRelative = 0.2f;
        [Space]
        [SerializeField, Range(0, 1)] 
        private float leanLerp = 0.8f;

        private Quaternion baseRotation;
        private Quaternion currentLocalLean;
        private Quaternion targetLocalLean;

        private void Start()
        {
            baseRotation = transform.localRotation;
        }

        private void FixedUpdate()
        {
            LeanTowards(rigidbody.velocity);
        }

        private void LeanTowards(Vector2 velocity)
        {
            float velocityMag = velocity.magnitude;
            float leanAngle = Mathf.Lerp(0, maxLean, Mathf.InverseLerp(0, maxVelocity, velocityMag));

            Vector3 leanDir = velocity.normalized;
            Vector3 leanAxis = Vector3.Cross(up, leanDir);

            Vector3 localLeanAxis = transform.parent.InverseTransformDirection(leanAxis);

            //limit lean on the local right axis by simple multiplication (attempt)
            leanAngle *= Mathf.Max(minimumLeanRelative, Mathf.Abs(Vector3.Dot(leanDir, transform.right)));

            targetLocalLean = Quaternion.AngleAxis(leanAngle, localLeanAxis);

            currentLocalLean = Quaternion.Lerp(currentLocalLean, targetLocalLean, leanLerp);

            transform.localRotation = currentLocalLean * baseRotation;
        }
    }
}
