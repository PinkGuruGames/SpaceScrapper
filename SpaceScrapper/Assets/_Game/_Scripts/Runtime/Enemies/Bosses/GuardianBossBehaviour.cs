using UnityEngine;
using SpaceScrapper;

namespace SpaceScrapper.Bosses
{
    internal abstract class GuardianBossBehaviour
    {
        //for transitions
        internal abstract void StateEnter(GuardianBoss guardian);
        internal abstract void StateLeave(GuardianBoss guardian);
        //exit conditions.
        internal abstract GuardianBossBehaviour MoveNext(GuardianBoss guardian);

        internal abstract void Move(GuardianBoss guardian);

        internal abstract void Aim(Transform target);
    }
}
