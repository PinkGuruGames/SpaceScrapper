using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A small drone that hovers around the area it started in, and flies in semi-random patterns to get in range of its target.
    /// </summary>
    [RequireComponent(typeof(SimpleEnemyEntity), typeof(CircleCollider2D))]
    public class HoveringDrone : AIControllerBase
    {
        [SerializeField, Header("Hovering Drone Settings")]
        private CircleCollider2D collider;
        [SerializeField]
        private float hoverRange = 15;
        [SerializeField]
        private float hoverAdjustRange = 7;
        [SerializeField, Tooltip("The time it takes to get from one position to the next.")]
        private float adjustPositionDuration = 1f;
        [SerializeField]
        private AnimationCurve adjustPositionCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField, Tooltip("After how many seconds (on average) to change your hover position")]
        private float adjustInterval = 5f;
        [SerializeField]
        private float hoverSpeed = 1f;
        [SerializeField]
        private float followSpeed = 3.5f;
        [SerializeField]
        private float backJumpAngle = 30;
        [SerializeField, Tooltip("Maximum distance, after which the drone will return to its starting position.")]
        private float leashRange;

        private Vector2 startPosition;
        private Vector2 currentPosition;
        private Vector2 targetPosition;
        private float distanceToTarget;
        private float leashSquareRange;

        //is this drone currently returning to its starting position?
        private bool returningToStartPos = false;

        private float destinationChangeTime;
        //the position the entity was at during the last destination change.
        private Vector2 destinationStartPos;

        private Vector2 destination;
        private Vector2 Destination 
        {
            get => destination;
            set 
            { 
                if(value != destination)
                {
                    destination = value;
                    destinationStartPos = currentPosition;
                    destinationChangeTime = Time.time;
                }
            } 
        }

        private float TimeSinceDestinationChange => Time.time - destinationChangeTime;

        protected override void Awake()
        {
            base.Awake();
            if (collider == null)
                collider = GetComponent<CircleCollider2D>();
            startPosition = transform.position;
            //pre-multiply some stuff
            leashSquareRange = leashRange * leashRange;
        }

        protected override void Aim()
        {
            //should hard aim on the target i guess.
        }

        protected override void Move()
        {
            currentPosition = transform.position;
            //return to the original position.
            if (returningToStartPos)
            {
                ReturnToStart();
                return;
            }
            //check leash range.
            CheckLeashRange();
            //only if a target exists.
            if (Target)
            {
                targetPosition = Target.transform.position;

                //continue towards destionation if within the time frame.
                if(TimeSinceDestinationChange < adjustPositionDuration)
                {
                    MoveToDestination();
                    return;
                }
                distanceToTarget = (targetPosition - currentPosition).magnitude;
                //if within attack range but relatively close to target, jump a little way away (GetPointInCone)
                if (distanceToTarget < hoverRange * 0.5f)
                {
                    float maxJump = hoverRange - distanceToTarget;
                    this.Destination = GetPointInCone(currentPosition, targetPosition, maxJump * 0.5f, maxJump, backJumpAngle);
                    return;
                }
                //Movement has many different goals:
                //if within attack range- just hover in place (very minimal movement)
                //and jump to a nearby spot every once in a while while keeping roughly the same distance
                //
                //if not within attack range, jump closer
                //if within attack range but far away jump to the result of CalculatePointNearTarget.
                //
                //if too far away from startPosition, clear target and return to it.
            }
            else
            {
                //No target, and sitting around the starting position.
            }
        }

        /// <summary>
        /// Move to the destination set via this.Destination. Uses an approximate Lerp with an animation curve.
        /// </summary>
        private void MoveToDestination()
        {
            //progress along the lerp
            float progress = TimeSinceDestinationChange / adjustPositionDuration;
            float t = adjustPositionCurve.Evaluate(progress);
            //evaluate the lerp 
            Vector2 nextPosition = Vector2.Lerp(destinationStartPos, destination, t);
            //if the path is NOT clear, limit the next position, and set the destinationChangeTime to -100 to prevent MoveToDestination from being run.
            RaycastHit2D hit;
            if (hit = Physics2D.CircleCast(currentPosition, collider.radius, nextPosition-currentPosition, 1, base.staticCollisionMask))
            {
                nextPosition = hit.centroid;
                destinationChangeTime = -100f;
            }
            //set position.
            transform.position = nextPosition;
            //Vector2 targetVelocity = delta / Time.deltaTime;
            //body.velocity = targetVelocity;
            
        }

        /// <summary>
        /// Move the drone back to its starting position.
        /// </summary>
        private void ReturnToStart()
        {
            Vector2 nextPosition = Vector2.MoveTowards(currentPosition, startPosition, Time.deltaTime * followSpeed);
            transform.position = nextPosition;
            if (Vector2.SqrMagnitude(nextPosition - startPosition) < 0.75f)
            {
                returningToStartPos = false;
            }
        }

        /// <summary>
        /// Check if the distance to the starting position is too big
        /// </summary>
        private void CheckLeashRange()
        {
            //if too far away from the starting position, clear target and return to it.
            if(Vector2.SqrMagnitude(startPosition - currentPosition) > leashSquareRange)
            {
                Target = null;
                returningToStartPos = true;
            }
        }

        protected override void CheckForEntities()
        {
            //ignore entities when returning to the starting position.
            if (returningToStartPos)
                return;
            base.CheckForEntities();
        }

        //I would kinda want to find a better solution for this, but this is the best i can do, and it should work flawlessly.
        /// <summary>
        /// Calculates a random point in the overlap of the range around two points in space.
        /// Largely taken from: paulbourke.net/geometry/circlesphere/ (as of 17. May 2022)
        /// </summary>
        /// <param name="p0">one of the points</param>
        /// <param name="r0">the radius of the allowed circle around p0</param>
        /// <param name="p1">the second point</param>
        /// <param name="r1">the radius of the allowed circle around p1</param>
        private Vector2 CalculatePointNearTarget(Vector2 p0, float r0, Vector2 p1, float r1)
        {
            //distance between the two points
            float d = distanceToTarget;
            //segment length from p0 to the center of the ellipsoid (overlap)
            float a = (r0 * r0 - r1 * r1 + d * d) / (2 * d);
            //the center of the ellipsoid
            Vector2 p2 = p0 + a * (p1 - p0) / d;

            //segment length of the center to a corner of the ellipsoid
            float h = Mathf.Sqrt(r0 * r0 - a * a);

            //x and y coordinates of one of the corners
            float x3 = p2.x + h * (p1.y - p0.y) / d;
            float y3 = p2.y + h * (p1.x - p0.x) / d;
            Vector2 p3 = new Vector2(x3, y3);

            //p3 = p2 + Vector2.Perpendicular(startToCenter.normalized) * h // would be possible too

            //vectors from the start to the other points
            Vector2 startToCorner = p3 - p0;
            Vector2 startToCenter = p2 - p0;
            //normalized start to center is startToCenter divided by its length, which is a.
            Vector2 startToCenterNormal = startToCenter / a;

            //the angle between them
            float angle = Vector2.Angle(startToCenter, startToCorner);
            //the minimum distance allowed.
            float minRange = Mathf.Pow(d - r1, 2);
            //maximum distance is the radius of the circle squared
            float maxRange = Mathf.Pow(r0, 2);

            //resample whenever the point lands outside the valid area of the ellipsoid.
            Vector2 destination = GetPointInArc();
            while(IsPointValid(destination) is false)
            {
                destination = GetPointInArc();
            }

            return destination;

            //Get a random point within the possible arc
            Vector2 GetPointInArc()
            {
                //random angle
                float randomAngle = Random.Range(-angle, angle);
                //distance was squared for more even distribution, need to take the root before continuing.
                float randomDistance = Mathf.Sqrt(Random.Range(minRange, maxRange));

                Vector2 offsetDirection = Quaternion.Euler(0, 0, randomAngle) * startToCenterNormal;

                Vector2 point = p0 + (offsetDirection * randomDistance);
                return point;
            }
            
            bool IsPointValid(Vector2 point)
            {
                //point is not allowed to be outside of the other circle (p1, r1)
                return (point - p1).sqrMagnitude < r1 * r1;
            }

        }

        /// <summary>
        /// Get a random point in a cone behind the position
        /// </summary>
        /// <param name="position">the current position</param>
        /// <param name="targetPosition">the position of the target that should be more distant</param>
        /// <param name="minDistance">the minimum of distance to travel</param>
        /// <param name="maxDistance">the maximum distance to travel</param>
        /// <param name="absoluteAngle">the angle limit</param>
        /// <returns></returns>
        private Vector2 GetPointInCone(Vector2 position, Vector2 targetPosition, float minDistance, float maxDistance, float absoluteAngle)
        {
            float angle = Random.Range(-absoluteAngle, absoluteAngle);
            float min = minDistance * minDistance;
            float max = maxDistance * maxDistance;
            float dist = Mathf.Sqrt(Random.Range(min, max));
            Vector2 offsetDirection = Quaternion.Euler(0, 0, angle) * (position - targetPosition).normalized;

            return position + offsetDirection * dist;
        }
    }
}
