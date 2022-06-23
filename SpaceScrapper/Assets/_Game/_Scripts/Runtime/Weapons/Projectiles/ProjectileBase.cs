using UnityEngine;
using SpaceScrapper;

namespace SpaceScrapper.Weapons
{
    /// <summary>
    /// Base class for projectiles fired from weapons.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class ProjectileBase : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D body;
        [SerializeField]
        private float lifetime;
        [SerializeField]
        private float speed;
        [SerializeField]
        protected GameObject hitParticlesPrefab;

        private float startTime;
        private ProjectilePool pool;

        public LivingEntity SourceEntity { get; private set; }
        public float Damage { get; private set; }
        public ProjectilePool Pool
        {
            get => pool;
            set
            {
                if (pool == null)
                    pool = value;
                else
                    Debug.LogWarning("Projectile already has a pool assigned, this shouldnt happen more than once.");
            }
        }

        protected Rigidbody2D Body => body;
        protected float Speed => speed;

        public virtual void Initialize()
        {
            if(body == null) 
                body = GetComponent<Rigidbody2D>();
        }

        protected abstract void Move();

        protected virtual void FixedUpdate()
        {
            Move();
            if(Time.time - startTime >= lifetime)
            {
                ReturnToPool();
            }
        }

        /// <summary>
        /// Deactivate the projectiles GameObject, and insert it back into the pool.
        /// </summary>
        protected void ReturnToPool()
        {
            gameObject.SetActive(false);
            pool.Insert(this);
            SpawnHitParticles();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            //other object is also a trigger, check tags before going further
            if(other.isTrigger)
            {
                //check whether the other object is also a projectile.
                ProjectileBase projectile = other.GetComponent<ProjectileBase>();
                if(projectile != null)
                {
                    ProcessProjectileHit(projectile);
                    return;
                }
            }
            //static collision => probably a wall
            else if(other.gameObject.isStatic)
            {
                ProcessStaticHit(other);
            }
            //other object is a collider
            //check for damageable now.
            IDamageable hitDamageable = other.GetComponent<IDamageable>();
            if(hitDamageable != null)
            {
                //1. check whether the damageable is an entity
                if (hitDamageable is LivingEntity otherEntity)
                {
                    if (otherEntity == SourceEntity)
                        return;
                    //hostility check
                    //non-hostile entities are only processed if the attack stems from the player.
                    if (SourceEntity == null || SourceEntity.CanDamage(otherEntity) || SourceEntity is PlayerEntity)
                    {
                        ProcessDamageEvent(hitDamageable, other);
                    }
                }
                //2. process any other damageables.
                else
                {
                    ProcessDamageEvent(hitDamageable, other);
                }
            }
            
        }

        /// <summary>
        /// Processes coming into contact with another projectile.
        /// </summary>
        /// <param name="other"></param>
        protected abstract void ProcessProjectileHit(ProjectileBase other);

        /// <summary>
        /// Process coming into context with a static collider or trigger.
        /// </summary>
        /// <param name="other">The collider that was hit.</param>
        protected virtual void ProcessStaticHit(Collider2D other)
        {
            ReturnToPool();
        }

        /// <summary>
        /// Process coming into contact with a damageable object. 
        /// Always deal damage, hostility check for LivingEntities has already been done.
        /// </summary>
        /// <param name="target">The target to apply damage to.</param>
        /// <param name="hitCollider">The collider that was found.</param>
        protected abstract void ProcessDamageEvent(IDamageable target, Collider2D hitCollider);

        /// <summary>
        /// Fire this projectile with these parameters
        /// </summary>
        /// <param name="source">The entity that fired this projectile</param>
        /// <param name="position">The starting position</param>
        /// <param name="direction">The direction in which to face</param>
        /// <param name="damage">The amount of damage as defined by the entity</param>
        public virtual void FireWithParameters(LivingEntity source, Vector2 position, Vector2 direction, float damage)
        {
            SourceEntity = source;
            Damage = damage;
            transform.position = position;
            transform.up = direction;
            startTime = Time.time;
            gameObject.SetActive(true);
        }

        /// <summary>
        /// Instantiates the hit particles prefab, and destroys it after 2 seconds.
        /// </summary>
        protected void SpawnHitParticles()
        {
            //this is super dirty but lol
            if (hitParticlesPrefab == null)
                return;
            var particles = Instantiate(hitParticlesPrefab, transform.position, Quaternion.identity);
            Destroy(particles, 2f);
        }

    }
}
