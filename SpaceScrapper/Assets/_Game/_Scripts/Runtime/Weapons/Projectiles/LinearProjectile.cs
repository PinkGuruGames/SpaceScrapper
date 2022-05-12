using UnityEngine;
using SpaceScrapper;

namespace SpaceScrapper.Weapons
{
    /// <summary>
    /// A simple projectile that flies in a straight line.
    /// </summary>
    public class LinearProjectile : ProjectileBase
    {
        protected override void Move()
        {
            //no changes for linear projectiles. they just fly via velocity.
        }

        protected override void ProcessDamageEvent(IDamageable target, Collider2D hitCollider)
        {
            //nothing special really.
            target.Damage(SourceEntity, Damage, hitCollider, false);
        }

        protected override void ProcessProjectileHit(ProjectileBase other)
        {
            //Linear projectiles should not interact with each other.
            if(other is LinearProjectile)
            {
                return;
            }
        }

        protected override void ProcessStaticHit(Collider2D other)
        {
            //just disable the object for now-- TODO
            gameObject.SetActive(false);
        }

        public override void FireWithParameters(LivingEntity source, Vector2 position, Vector2 direction, float damage)
        {
            base.FireWithParameters(source, position, direction, damage);
            //set velocity.
            Body.velocity = direction * Speed;
        }
    }
}
