﻿using System;
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
            Body.rotation = angle;
            Body.velocity = (Quaternion.Euler(0, 0, angle) * Vector3.up) * base.Speed;
        }
    }
}