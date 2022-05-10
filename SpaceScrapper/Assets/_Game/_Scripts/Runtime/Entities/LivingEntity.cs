using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// The base class for all living entities that have health.
    /// </summary>
    public abstract class LivingEntity : MonoBehaviour, IDamageable
    {
        private float currentHealth;
        private float maxHealth;

        /// <summary>
        /// The current amount of health this entity has. public get, protected set.
        /// Can be overridden if necessary.
        /// </summary>
        public virtual float CurrentHealth
        {
            get => currentHealth;
            protected set
            {
                if (value <= 0)
                    Die();
                currentHealth = Mathf.Clamp(value, 0, maxHealth);
            }
        }

        /// <summary>
        /// The maximum amount of health this entity can have. public get, protected set.
        /// </summary>
        public float MaxHealth
        {
            get => maxHealth;
            protected set => maxHealth = value;
        }

        /// <inheritdoc></inheritdoc>
        public abstract void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false);

        /// <summary>
        /// Method that defines how the entity dies.
        /// </summary>
        protected abstract void Die();
    }
}
