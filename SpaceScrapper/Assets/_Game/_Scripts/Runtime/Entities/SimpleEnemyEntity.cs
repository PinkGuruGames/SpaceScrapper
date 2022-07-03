using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// Defines a simple enemy entity that is usually only comprised of a single collider.
    /// This has very limited functionality because it doesnt need to do much.
    /// </summary>
    public class SimpleEnemyEntity : LivingEntity
    {
        public override void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false)
        {
            CurrentHealth -= damage;
            ShowHitFlash();
        }

        /// <summary>
        /// Method that defines how the entity dies. Is called automatically by LivingEntity.CurrentHealth, when the value is <= 0.
        /// </summary>
        protected override void Die()
        {
            base.Die();
            //WIP:
            gameObject.SetActive(false);
            //throw new System.NotImplementedException();
        }
    }
}
