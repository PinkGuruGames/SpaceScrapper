using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// The baseclass for all AI controlled units, enemies and other.
    /// </summary>
    //[RequireComponent(typeof(LivingEntity))]
    public abstract class AIControllerBase : MonoBehaviour
    {
        [Header("General AI Settings")]
        [SerializeField, Tooltip("How many frames are between each entity check for targetting")]
        private int entityCheckFrameInterval = 1;

        [SerializeField]
        private LayerMask entityCheckMask;
        [SerializeField]
        protected LayerMask staticCollisionMask;
        [SerializeField]
        private float entityCheckRange = 20;

        private LivingEntity entityComponent;

        /// <summary>
        /// The target entity
        /// </summary>
        private LivingEntity target;

        //a buffer of colliders to use for physics overlap checks without allocating anything.
        protected readonly Collider2D[] colliderBuffer = new Collider2D[10];

        //the last frame on which an entity check was performed.
        private int lastEntityCheck;

        protected LivingEntity EntityComponent => entityComponent;
        protected LayerMask EntityCheckMask => entityCheckMask;
        public LivingEntity Target
        {
            get => target;
            protected set
            {
                if (target != null)
                    target.OnEntityDied -= OnTargetDied;
                if (value != null)
                    value.OnEntityDied += OnTargetDied;
                target = value;
            }
        }

        /// <summary>
        /// Method that exclusively handles movement.
        /// </summary>
        protected abstract void Move();

        /// <summary>
        /// Method that hanldes aim (rotation) and enables weapons if necessary.
        /// Runs after Move.
        /// </summary>
        protected abstract void Aim();

        protected virtual void Awake()
        {
            entityComponent = GetComponent<LivingEntity>();
        }

        protected virtual void Update()
        {
            Move();
            Aim();
            //only check for entites every X frames.
            if(Time.frameCount >= lastEntityCheck + entityCheckFrameInterval)
            {
                lastEntityCheck = Time.frameCount;
                //only need to search for a target if we dont have one already
                if(target == null)
                    CheckForEntities();
            }
        }

        /// <summary>
        /// Checks for entities to attack.
        /// </summary>
        protected virtual void CheckForEntities()
        {
            int objectsFound = Physics2D.OverlapCircleNonAlloc(transform.position, entityCheckRange, colliderBuffer, EntityCheckMask);
            LivingEntity otherEntity;
            for(int i = 0; i < objectsFound; i++)
            {
                otherEntity = colliderBuffer[i].GetComponentInParent<LivingEntity>();
                //check if the collider is part of a living entity
                if(otherEntity && this.EntityComponent.IsHostileTowards(otherEntity))
                {
                    //TODO: Line of sight check- there should be no *static* collision between the two entities.
                    //Atm only set target to the found one if there is no target at all.
                    Target = otherEntity;
                    return;
                }
            }
        }

        /// <summary>
        /// Event callback for when the target of this AI died.
        /// Clears Target and resets entity check timer by default.
        /// </summary>
        protected virtual void OnTargetDied()
        {
            Target = null;
            lastEntityCheck = Time.frameCount;
        }

        /// <summary>
        /// Checks if the direct path between two positions is free from static collision
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="hit"></param>
        /// <returns>True if there is no collision.</returns>
        protected bool PathIsClear(Vector2 from, Vector2 to)
        {
            return !Physics2D.Linecast(from, to, staticCollisionMask);
        }
    }
}