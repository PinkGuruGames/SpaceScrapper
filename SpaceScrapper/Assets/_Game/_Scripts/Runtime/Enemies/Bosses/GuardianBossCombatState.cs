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

            //check for overlap with collision through collision contacts, try to get out of them.
            ContactPoint2D[] contacts = guardian.GetContacts(out int n);
            if (n > 0)
            {
                //just a 0 0 vector for velocity that will be added to.
                Vector2 vel = default;
                for(int i = 0; i < n; i++)
                {
                    //add the contact normal to the velocity. this should hopefully make it move out of collision.
                    vel += contacts[i].normal;
                }
            }
            else
            {
                guardian.Body.velocity = Vector2.zero;
            }
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