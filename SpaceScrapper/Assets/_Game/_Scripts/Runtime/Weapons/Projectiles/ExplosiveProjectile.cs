using UnityEngine;

namespace SpaceScrapper.Weapons
{
    public class ExplosiveProjectile : ProjectileBase
    {
        protected override void Move()
        {
            //throw new System.NotImplementedException();
        }

        protected override void ProcessDamageEvent(IDamageable target, Collider2D hitCollider)
        {
            Explode();
        }

        protected override void ProcessProjectileHit(ProjectileBase other)
        {
            //Explosive Projectiles can be made to detonate by enemy linear projectiles.
            if(SourceEntity.CanDamage(other.SourceEntity) && other is LinearProjectile)
                Explode();
        }

        protected override void ProcessStaticHit(Collider2D other)
        {
            Explode();
        }

        protected void Explode()
        {
            throw new System.NotImplementedException();
        }
    }
}
