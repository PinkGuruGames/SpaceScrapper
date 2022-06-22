using System;
using UnityEngine;

namespace SpaceScrapper.Weapons
{
    public class HomingMissile : ExplosiveProjectile
    {
        [SerializeField]
        private float turnSpeed;

        private Transform target;
        public override void FireWithParameters(LivingEntity source, Vector2 position, Vector2 direction, float damage)
        {
            base.FireWithParameters(source, position, direction, damage);
            //this implementation currently limits homing missiles to be for AI entities only.
            var ai = source.GetComponent<AIControllerBase>();
            target = ai.Target.transform;
        }

        protected override void Move()
        {
            //always follow target.
            Vector2 offset = (Vector2)target.position - Body.position;
            float angle = Vector2.SignedAngle(Vector2.up, offset);

            float angleOffset = angle - Body.rotation;
            float sign = Mathf.Sign(angleOffset);
            float absAngle = Mathf.Abs(angleOffset);

            //check if we are accidentally trying the longer rotation.
            if(absAngle > 180)
            {
                absAngle = 360 - absAngle;
                sign *= -1;
            }

            float deltaUnsigned = Mathf.Min(Time.deltaTime * turnSpeed, absAngle);
            float deltaSigned = deltaUnsigned * sign;

            Body.rotation += deltaSigned;
            Body.velocity = (Quaternion.Euler(0, 0, Body.rotation) * Vector3.up) * base.Speed;
        }
    }
}
