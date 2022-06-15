using System.Collections;
using System;
using UnityEngine;
using SpaceScrapper.Weapons;

namespace SpaceScrapper.Bosses
{
    /// <summary>
    /// Behaviour for the guardian boss in the demo level.
    /// </summary>
    public class GuardianBoss : AIControllerBase
    {
        [SerializeField]
        private Rigidbody2D body;
        [SerializeField]
        private Transform[] weaponTransforms;
        [SerializeField]
        private Weapon[] gunWeapons;
        [SerializeField]
        private Weapon rocketLauncher;
        [SerializeField]
        private new CompositeCollider2D collider;

        //state behaviours can be serialized in case there is a need for settings that are specific to one.
        //[SerializeField]
        //private T:GuardianBossBehaviour serializedState;
        [Header("State Specific Settings")]
        [SerializeField]
        private GuardianBossIdleState idleState;
        [SerializeField]
        private GuardianBossCombatState combatState;
        [SerializeField]
        private GuardianBossDodgeAttackState dodgeAttackState;

        private GuardianBossBehaviour activeState;

        private GuardianBossBehaviour ActiveState
        {
            get => activeState;
            set
            {
                if (activeState != value)
                    value.StateEnter(this);
                activeState = value;
            }
        }

        //Get-only properties for the states to work.
        internal GuardianBossCombatState CombatState => combatState;
        internal GuardianBossDodgeAttackState DodgeAttackState => dodgeAttackState;

        internal Rigidbody2D Body => body;

        //buffers to avoid allocating
        RaycastHit2D[] raycastHitBuffer = new RaycastHit2D[10];

        protected override void Awake()
        {
            base.Awake();
            //default to idle.
            ActiveState = idleState;
        }

        protected override void Aim()
        {
            if (Target == null)
                return;

            //target transform: tt
            var tt = Target.transform;
            //the "up" direction in idle position is the same for the guns and the boss itself.
            Vector2 up = transform.up;

            //body.rotation = Vector2.SignedAngle(Vector2.up, Target.transform.position - transform.position);
            for(int i = 0; i < weaponTransforms.Length; i++)
            {
                //weapon transform: tt
                var wt = weaponTransforms[i];

                var offset = tt.position - wt.position;

                var angle = Vector2.SignedAngle(up, offset);

                wt.localEulerAngles = new Vector3(0, 0, angle);

            }
        }

        protected override void Update()
        {
            //dont call base.Update because it would duplicate Aim and Move falsely.
            Aim();
            ActiveState = ActiveState.MoveNext(this);
        }

        protected void FixedUpdate()
        {
            Move();
        }

        //required method, might aswell use it.
        protected override void Move()
        {
            ActiveState.Move(this);
        }

        //Methods that expose functionality to GuardianBossBehaviour state classes.
        //necessary to avoid having to expose everything as a seperate property, when this limited functionality is all thats required.

        /// <summary>
        /// Causes BOTH guns that are attached to the boss to fire.
        /// </summary>
        internal void FireGuns()
        {
            foreach(var w in gunWeapons)
            {
                w.ToggleShooting();
            }
        }

        /// <summary>
        /// Activates the rocket launcher.
        /// </summary>
        internal void LaunchRockets()
        {
            rocketLauncher.ToggleShooting();
        }

        /// <summary>
        /// Performs a target search based on the AIControllerBase check.
        /// <see cref="AIControllerBase.CheckForEntities"/>
        /// </summary>
        internal void CheckForTarget()
        {
            base.CheckForEntities();
        }

        /// <summary>
        /// Rotates the boss to face the target.
        /// </summary>
        internal void FaceTarget()
        {
            Vector2 offset = (Vector2)Target.transform.position - body.position;
            float angle = Vector2.SignedAngle(Vector2.up, offset);
            body.rotation = angle;
        }

        /// <summary>
        /// Casts the collider of the GuardianBoss in the specified direction with the given distance.
        /// </summary>
        /// <param name="direction">The direction, should be normalized.</param>
        /// <param name="distance">The distance in units.</param>
        /// <returns>ReadOnlySpan of all raycasts hit of the collider.Cast operation.</returns>
        internal ReadOnlySpan<RaycastHit2D> CastCollider(Vector2 direction, float distance)
        {
            int hitCount = collider.Cast(direction, raycastHitBuffer, distance);
            return new ReadOnlySpan<RaycastHit2D>(raycastHitBuffer, 0, hitCount);
        }
    }
}
