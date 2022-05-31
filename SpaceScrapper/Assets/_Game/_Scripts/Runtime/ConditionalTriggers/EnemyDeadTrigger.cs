using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A conditional trigger that runs when all specified entities have died.
    /// </summary>
    public class EnemyDeadTrigger : MonoBehaviour
    {
        [SerializeField]
        private LivingEntity[] entities;

        private int count = 0;
        private ITriggerable[] triggerables;

        // Start is called before the first frame update
        void Start()
        {
            foreach(var e in entities)
            {
                e.OnEntityDied += OnEntityDied;
            }
            //Get Triggerables from same object or children.
            triggerables = GetComponentsInChildren<ITriggerable>();
        }

        private void OnEntityDied()
        {
            count++;
            if(count >= entities.Length)
            {
                foreach(var t in triggerables)
                {
                    t.Trigger();
                }
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            if(entities != null && entities.Length > 0)
            foreach(var e in entities)
            {
                if(e != null)
                    Gizmos.DrawSphere(e.transform.position, 1);
            }
        }
#endif
    }
}
