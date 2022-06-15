using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper.Bosses
{
    /// <summary>
    /// Defines the behaviour for the ramming /ram attack of the guardian boss.
    /// Starts with a short warmup before flying in a set direction until a distance is reached or static collision is hit.
    /// </summary>
    [Serializable]
    internal class GuardianBossRamAttackState : GuardianBossBehaviour
    {
        [SerializeField]
        private float ramSpeed = 35f;
        [SerializeField]
        private float chargeRange = 150f;
        [SerializeField]
        private float windupTime = 1.2f;
        [SerializeField]
        private float collisionStaggerTime = 1.2f;
        [SerializeField]
        private float chargeDamage = 7f;

        private Vector2 headingDirection;
        private List<Collider2D> ignoredColliders = new List<Collider2D>(10);

        private bool shouldStop = false;
        private float stopTime = float.PositiveInfinity;

        private bool isDone;
        private float traveledDistance = 0;

        internal override void Move(GuardianBoss guardian)
        {
            if (shouldStop)
            {
                //override velocity with zero.
                guardian.Body.velocity = Vector2.zero;
            }
            else
            {
                //always reset the velocity, so it stays "constant"
                guardian.Body.velocity = headingDirection * ramSpeed;
                //increase travelled distance preemptively.
                traveledDistance += ramSpeed * Time.deltaTime;

                ProcessCollisionContacts(guardian);

                if (traveledDistance >= chargeRange)
                    isDone = true;
            }
        }

        //Reset things in state enter.
        internal override void StateEnter(GuardianBoss guardian)
        {
            //reset stop condition.
            shouldStop = false;
            stopTime = float.PositiveInfinity;
            traveledDistance = 0;
            isDone = false;
            guardian.FaceTarget();
            headingDirection = guardian.transform.up;
            base.StateEnter(guardian);
        }

        private void ProcessCollisionContacts(GuardianBoss guardian)
        {
            //get the contacts.
            ContactPoint2D[] contactBuffer = new ContactPoint2D[10];
            int x = guardian.Body.GetContacts(contactBuffer);
            for (int i = 0; i < x; i++)
            {
                var contact = contactBuffer[i];
                //check for static collision first.
                Collider2D other = contact.collider;
                if (other.gameObject.isStatic)
                {
                    //woop woop static collision hit.
                    //should still process other contacts tho
                    shouldStop = true;
                    stopTime = Time.time;
                    //DO SOMETHING WITH contact.normal ??;
                    continue;
                }
                //ignore collision between the hit collider and the guardians own collider.
                ignoredColliders.Add(other);
                Physics2D.IgnoreCollision(guardian.Collider, other);

                //process damage events.
                IDamageable damageable = other.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    if (damageable is LivingEntity entity)
                    {
                        if (guardian.EntityComponent.CanDamage(entity))
                        {
                            //damage the entity.
                            entity.Damage(guardian.EntityComponent, chargeDamage, guardian.Collider, true);
                        }
                    }
                    else //just a regular damageable object.
                    {
                        damageable.Damage(guardian.EntityComponent, chargeDamage, guardian.Collider, true);
                    }
                }
            }
        }

        internal override GuardianBossBehaviour MoveNext(GuardianBoss guardian)
        {
            //check exit condition back to combat state.
            if (shouldStop && Time.time - stopTime > collisionStaggerTime || isDone)
                return guardian.CombatState;
            return this;
        }
    }
}
