using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A conditional trigger that runs when all specified entities have died.
    /// </summary>
    public class EnemyDeadTrigger : ConditionalTrigger
    {
        [SerializeField]
        private LivingEntity[] entities;

        private int count = 0;

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            foreach(var e in entities)
            {
                e.OnEntityDied += OnEntityDied;
            }
        }

        private void OnEntityDied()
        {
            count++;
            if(count >= entities.Length)
            {
                TriggerAll();
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
