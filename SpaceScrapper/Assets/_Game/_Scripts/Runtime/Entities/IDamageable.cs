using UnityEngine;

namespace SpaceScrapper
{
    public interface IDamageable
    {
        /// <summary>
        /// Method for dealing damage to this object.
        /// </summary>
        /// <param name="source">The LivingEntity that is the cause for damage.</param>
        /// <param name="damage">The amount of damage to be dealt.</param>
        /// <param name="sourceCollider">The Collider2D that was hit (first) when the attack hit.</param>
        /// <param name="ignoreWeakspot">Whether to ignore weakspot vulnerabilities on this attack.</param>
        public void Damage(LivingEntity source, float damage, Collider2D sourceCollider, bool ignoreWeakspot = false);
    }
}