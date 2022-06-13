using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A simple entity that is part of another. 
    /// It has its own health pool and can be destroyed individually, 
    /// but also applies the damage dealt to another entity, it belongs to.
    /// </summary>
    public class PartialEntity : LivingEntity
    {
        [SerializeField]
        private SimpleEnemyEntity superEntity;

        public override void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false)
        {
            this.CurrentHealth -= damage;
            superEntity.Damage(source, damage, sourceCollider, ignoreWeakspot);
        }

        protected override void Die()
        {
            base.Die();
            gameObject.SetActive(false);
        }
    }
}
