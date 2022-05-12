using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This serves the purpose of handling incoming damage, and handling how players die and respawn.
    /// </summary>
    public class PlayerEntity : LivingEntity
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
