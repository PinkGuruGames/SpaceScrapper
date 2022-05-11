using UnityEngine;
using System;

namespace SpaceScrapper
{
    /// <summary>
    /// The base class for all living entities that have health.
    /// </summary>
    public abstract class LivingEntity : MonoBehaviour, IDamageable
    {
        [SerializeField]
        private Faction faction;

        private float currentHealth;
        private float maxHealth;

        public event Action OnEntityDied;

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
        protected virtual void Die()
        {
            OnEntityDied();
        }

        /// <summary>
        /// Checks if one entity is hostile towards another. Used in enemy AI behaviour.
        /// </summary>
        /// <param name="other">the other entity to check</param>
        /// <returns>True when the entities belong to different factions. (WIP)</returns>
        public virtual bool IsHostileTowards(LivingEntity other)
        {
            return other.faction != this.faction;
        }

    }
}
