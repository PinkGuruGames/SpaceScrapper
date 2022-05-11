using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A simple enemy type that does not move, and only rotates to track its target.
    /// </summary>
    public class StationaryTurret : AIControllerBase
    {
        protected override void Aim()
        {
            throw new System.NotImplementedException();
        }

        protected override void Move()
        {
            //Leave empty. Stationary turrets dont move, because well, they're stationary.
        }
    }
}
