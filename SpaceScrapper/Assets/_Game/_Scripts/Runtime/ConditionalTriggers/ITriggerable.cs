using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// Interface for objects that can be triggered, and have some sort of binary state. e.g. Doors that can be closed or opened.
    /// </summary>
    public interface ITriggerable
    {
        void Trigger();
    }
}
