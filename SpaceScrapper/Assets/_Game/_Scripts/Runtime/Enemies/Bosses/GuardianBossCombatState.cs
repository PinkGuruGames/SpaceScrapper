using UnityEngine;
using System;

namespace SpaceScrapper.Bosses
{
    [Serializable]
    public class GuardianBossCombatState : GuardianBossBehaviour
    {
        [SerializeField, Tooltip("Activates a special ability every x seconds.")]
        private float abilityTimer = 5f;

        internal override void Move(GuardianBoss guardian)
        {
            //this is FixedUpdate, but launching rockets shouldnt be an issue in here. the timers work either way.
            guardian.LaunchRockets();
            //default movement pattern here:

        }

        internal override GuardianBossBehaviour MoveNext(GuardianBoss guardian)
        {
            if(Time.time - EnterTime >= abilityTimer)
            {
                return guardian.DodgeAttackState; //return special ability state.
            }
            return this;
        }
    }
}