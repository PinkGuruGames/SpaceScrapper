using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// Base class for conditional triggers that activate all ITriggerables on the same GameObject or its children.
    /// Used to avoid having to write a bunch of this code again.
    /// </summary>
    public abstract class ConditionalTrigger : MonoBehaviour
    {
        private ITriggerable[] triggerables;

        protected virtual void Start()
        {
            triggerables = GetComponentsInChildren<ITriggerable>();
        }

        protected void TriggerAll()
        {
            foreach (var t in triggerables)
                t.Trigger();
        }
    }
}
