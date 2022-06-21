using UnityEngine;

namespace SpaceScrapper.Weapons
{
    public class ExplosiveProjectile : ProjectileBase
    {
        [SerializeField]
        private float explosionRadius;

        private Collider2D[] colliderBuffer = new Collider2D[10];

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
            if (SourceEntity.CanDamage(other.SourceEntity) && other is LinearProjectile lp)
            {
                Explode();
            }
        }

        protected override void ProcessStaticHit(Collider2D other)
        {
            Explode();
        }

        protected void Explode()
        {
            int count = Physics2D.OverlapCircleNonAlloc(transform.position, explosionRadius, colliderBuffer);
            for(int i = 0; i < count; i++)
            {
                var c = colliderBuffer[i];
                if (c.gameObject.isStatic)
                    continue;
                IDamageable id = c.GetComponent<IDamageable>();
                if (id != null)
                {
                    id.Damage(SourceEntity, this.Damage, c, true);
                }
            }
        }
    }
}
