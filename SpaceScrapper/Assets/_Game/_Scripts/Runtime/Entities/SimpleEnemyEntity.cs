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
            throw new System.NotImplementedException();
        }

        protected override void Die()
        {
            base.Die();
            throw new System.NotImplementedException();
        }
    }
}
