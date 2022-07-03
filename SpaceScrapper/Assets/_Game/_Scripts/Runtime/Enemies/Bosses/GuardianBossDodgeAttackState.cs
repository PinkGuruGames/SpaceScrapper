using System;
using System.Collections;
using UnityEngine;

namespace SpaceScrapper.Bosses
{
    [Serializable]
    public class GuardianBossDodgeAttackState : GuardianBossBehaviour
    {
        [SerializeField, Range(1f, 40f)]
        private float dodgeDistance = 6f;
        [SerializeField, Range(0.1f, 2f)]
        private float dodgeTime = 1f;
        [SerializeField, Range(0.1f, 1f)]
        private float dodgeInterval = 0.2f;
        [SerializeField]
        private AnimationCurve dodgeCurve;

        ///<summary>positions: { firstTarget, secondTarget, startPosition }</summary>
        private Vector2[] positions = new Vector2[3];

        private float totalTime;

        internal override void Move(GuardianBoss guardian)
        {
            guardian.FaceTarget();
            //because the dodge attack is defined via the Co_Dodge Coroutine, which waits for FixedUpdates.
        }

        internal override GuardianBossBehaviour MoveNext(GuardianBoss guardian)
        {
            if(Time.time - EnterTime >= totalTime)
            {
                //return to default combat when this attack is over.
                return guardian.CombatState;
            }
            return this;
        }

        internal override void StateEnter(GuardianBoss guardian)
        {
            //recalculate totalTime before doing anything.
            totalTime = 4 * dodgeInterval + 3 * dodgeTime + Time.fixedDeltaTime;
            //calculate the starting positions, then start the coroutine.
            CalculateTargetLocations(guardian);
            guardian.StartCoroutine(Co_Dodge(guardian));
            base.StateEnter(guardian);
        }

        /// <summary>
        /// Calculate the target locations of the dodges.
        /// </summary>
        /// <param name="guardian"></param>
        private void CalculateTargetLocations(GuardianBoss guardian)
        {
            var transform = guardian.transform;
            positions[2] = guardian.transform.position;
            Vector2 right = transform.right;

            //simple loop that combines the cast to the right and to the left, as to not have duplicate code.
            for (int i = 0; i <= 1; i++)
            {
                ReadOnlySpan<RaycastHit2D> hits = guardian.CastCollider(right, dodgeDistance);
                if (hits.Length is 0)
                {
                    positions[i] = positions[2] + right * dodgeDistance;
                }
                else
                {
                    positions[i] = hits[0].centroid;
                }
                //invert right.
                right = -right;
            }
        }

        //i know this is dirty, but its not really a variable, so idc
        private readonly WaitForFixedUpdate waiter = new WaitForFixedUpdate();
        /// <summary>
        /// Perform the dodge attack action.
        /// </summary>
        IEnumerator Co_Dodge(GuardianBoss guardian)
        {
            yield return waiter;
            //TODO: actual telegraphing instead of just sitting there for a split second.
            for (int i = 0; i < positions.Length; i++)
            {
                //perform the actual dodge.
                for(float t = 0; t < dodgeTime; t += Time.deltaTime)
                {
                    float progress = t / dodgeTime;

                    //if previous index is out of bounds, wrap around to max.
                    Vector2 start = positions[i - 1 < 0 ? positions.Length - 1 : i - 1];
                    Vector2 goal = positions[i];

                    //evaluate the animation curve.
                    float lerpValue = dodgeCurve.Evaluate(progress);

                    //get the target position by lerping from start to goal via lerpValue 
                    Vector2 targetPosition = Vector2.Lerp(start, goal, lerpValue);
                    Vector2 currentPosition = guardian.transform.position;

                    //offset and distance from current to target position.
                    Vector2 offset = targetPosition - currentPosition;
                    float distance = offset.magnitude;
                    if (Mathf.Approximately(distance, 0))
                        continue;
                    //the projected speed required to travel offset distance in one physics frame.
                    float speed = distance / Time.deltaTime;
                    Vector2 direction = offset / distance;
                    Vector2 velocity = direction * speed;

                    //assign velocity.
                    guardian.Body.velocity = velocity;

                    yield return waiter;
                }
                guardian.FireGuns();
                //wait a short interval.
                for (float t = 0; t < dodgeInterval; t += Time.deltaTime)
                    yield return waiter;
            }
        }
    }
}
