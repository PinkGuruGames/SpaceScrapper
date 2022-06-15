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
            guardian.FaceTarget();
            guardian.Body.velocity = Vector2.zero;
        }

        internal override GuardianBossBehaviour MoveNext(GuardianBoss guardian)
        {
            if(Time.time - EnterTime >= abilityTimer)
            {
                if (UnityEngine.Random.value < 0.5f)
                    return guardian.DodgeAttackState; //return special ability state.
                else
                    return guardian.RamAttackState;
            }
            return this;
        }
    }
}