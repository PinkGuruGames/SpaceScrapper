using UnityEngine;

namespace SpaceScrapper
{
    public class SimpleEnemyEntity : LivingEntity
    {
        public override void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false)
        {
            throw new System.NotImplementedException();
        }

        protected override void Die()
        {
            throw new System.NotImplementedException();
        }
    }
}
