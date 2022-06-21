using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This serves the purpose of handling incoming damage, and handling how players die and respawn.
    /// </summary>
    public class PlayerEntity : LivingEntity
    {
        [SerializeField]
        private float defaultHealth = 100;

        private void Start()
        {
            InitializeWithHealth(defaultHealth);
        }

        public override void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false)
        {
            //throw new System.NotImplementedException();
            CurrentHealth -= damage;
            ShowHitFlash();
        }

        /// <summary>
        /// Method that defines how the entity dies. Is called automatically by LivingEntity.CurrentHealth, when the value is <= 0.
        /// </summary>
        protected override void Die()
        {
            base.Die();
            throw new System.NotImplementedException();
        }
    }
}
