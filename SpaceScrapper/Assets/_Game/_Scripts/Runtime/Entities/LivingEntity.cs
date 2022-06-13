using UnityEngine;
using System;

namespace SpaceScrapper
{
    /// <summary>
    /// The base class for all living entities that have health.
    /// </summary>
    public abstract class LivingEntity : MonoBehaviour, IDamageable
    {
        [SerializeField, Tooltip("The Faction this Entity belongs to.")]
        private Faction faction;
        [SerializeField]
        private float defaultMaxHealth = 2f;

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
            private set => maxHealth = value;
        }

        //set the current and max health to the default max health value.
        protected virtual void Awake()
        {
            maxHealth = currentHealth = defaultMaxHealth;
        }

        /// <inheritdoc></inheritdoc>
        public abstract void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false);

        /// <summary>
        /// Method that defines how the entity dies. Is called automatically by LivingEntity.CurrentHealth, when the value is <= 0.
        /// </summary>
        protected virtual void Die()
        {
            OnEntityDied?.Invoke();
        }

        /// <summary>
        /// Checks if one entity is hostile towards another. Used in enemy AI behaviour.
        /// </summary>
        /// <param name="other">the other entity to check</param>
        /// <returns>True when the entities belong to different factions. (WIP)</returns>
        public virtual bool IsHostileTowards(LivingEntity other)
        {
            return faction.IsHostileTowards(other.faction);
        }

        /// <summary>
        /// Check if this entity is allowed to hurt another.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public virtual bool CanDamage(LivingEntity other)
        {
            return faction.CanDamage(other.faction);
        }

        /// <summary>
        /// Check if this entity should be hostile towards another, and whether it can damage it.
        /// </summary>
        /// <param name="other"></param>
        /// <param name="canAttack"></param>
        /// <returns></returns>
        public virtual bool IsHostileAndCanAttack(LivingEntity other, out bool canAttack)
        {
            return faction.IsHostileAndCanAttack(other.faction, out canAttack);
        }

        /// <summary>
        /// Sets the max health to the current value, and the current health equal to that.
        /// Should therefore not be used outside of Start() or similar methods.
        /// </summary>
        /// <param name="maxHealth"></param>
        protected virtual void InitializeWithHealth(float maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
    }
}
