using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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

        [SerializeField, Header("Rendering")]
        private MeshRenderer[] renderers;
        [SerializeField]
        private Material hitFlashMaterial;
        [SerializeField, Range(0.05f, 0.5f)]
        private float hitFlashDuration = 0.05f;

        private float currentHealth;
        private float maxHealth;

        private Coroutine hitFlashRoutine;
        private Dictionary<MeshRenderer, Material> materials;

        public event Action OnEntityDied;
        public event Action<float> OnHealthChanged;
        public event Action<float> OnMaxHealthChanged;
        public event Action OnBecameActive;

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
                float clampedHealth = Mathf.Clamp(value, 0, maxHealth);
                if (clampedHealth != currentHealth)
                {
                    currentHealth = clampedHealth;
                    OnHealthChanged?.Invoke(clampedHealth);
                }
            }
        }

        /// <summary>
        /// The maximum amount of health this entity can have. public get, protected set.
        /// </summary>
        public float MaxHealth
        {
            get => maxHealth;
            private set
            {
                if (value != maxHealth)
                    OnMaxHealthChanged?.Invoke(value);
                maxHealth = value;
            }
        }

        //set the current and max health to the default max health value.
        protected virtual void Awake()
        {
            maxHealth = currentHealth = defaultMaxHealth;
            //set up the dictionary.
            materials = new Dictionary<MeshRenderer, Material>(renderers.Length);
            foreach(MeshRenderer renderer in renderers)
            {
                materials.Add(renderer, renderer.material);
            }
        }

        private void OnEnable()
        {
            OnBecameActive?.Invoke();
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

        /// <summary>
        /// Shows a hit flash by swapping out the material of the renderers
        /// </summary>
        protected void ShowHitFlash()
        {
            if (hitFlashRoutine != null)
            {
                StopCoroutine(hitFlashRoutine);
                hitFlashRoutine = null;
            }
            if(CurrentHealth > 0) //only if not already dead.
                hitFlashRoutine = StartCoroutine(Co_HitFlash());
        }

        /// <summary>
        /// coroutine that swaps out the materials of the renderers to produce a "hitflash" effect.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Co_HitFlash()
        {
            //apply hitflash material.
            foreach(var r in renderers)
            {
                r.sharedMaterial = hitFlashMaterial;
            }
            yield return new WaitForSeconds(hitFlashDuration);
            //re-assign the original material. but via sharedMaterial to avoid instancing new materials.
            foreach(var r in renderers)
            {
                r.sharedMaterial = materials[r];
            }
        }
    }
}
