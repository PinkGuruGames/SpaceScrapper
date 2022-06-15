using UnityEngine;
using System;

namespace SpaceScrapper.Bosses
{
    [Serializable]
    public abstract class GuardianBossBehaviour
    {
        protected float EnterTime { get; private set; }

        //for transitions
        internal virtual void StateEnter(GuardianBoss guardian)
        {
            EnterTime = Time.time;
        }

        //exit conditions.
        internal abstract GuardianBossBehaviour MoveNext(GuardianBoss guardian);

        /// <summary>
        /// Move the guardian boss. This is called in FixedUpdate due to relying on a Rigidbody2D.
        /// </summary>
        internal abstract void Move(GuardianBoss guardian);
    }

    /// <summary>
    /// Idle doesnt really do much.
    /// </summary>
    [Serializable]
    public class GuardianBossIdleState : GuardianBossBehaviour
    {
        internal override void Move(GuardianBoss guardian)
        {
            return;
        }

        internal override GuardianBossBehaviour MoveNext(GuardianBoss guardian)
        {
            guardian.CheckForTarget();
            if(guardian.Target)
            {
                return guardian.CombatState;
            }
            return this;
        }
    }
}
