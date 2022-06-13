using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// Behaviour for the guardian boss in the demo level.
    /// </summary>
    public class GuardianBoss : AIControllerBase
    {
        [SerializeField]
        private Rigidbody2D body;
        [SerializeField]
        private Transform[] weaponTransforms;
        [SerializeField]
        private Weapons.Weapon[] weapons;

        protected override void Aim()
        {
            if (Target)
                body.rotation = Vector2.SignedAngle(Vector2.up, Target.transform.position - transform.position);
        }

        protected override void Move()
        {

        }
    }
}
