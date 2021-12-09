﻿using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This controls a weapon that shoot a continues burst of bullets or a single bullet
    /// </summary>
    public class BulletWeaponController : WeaponController
    {
        [Header("Inifnite = -1")]
        [SerializeField] private int shootsPerTriggerHold = -1;
        [SerializeField] private float shootInterval = 0.1f;

        private float timeSinceLastShot = -1;
        private int shotsInSeries = 0;

        public override bool WantsToShoot
        {
            set
            {
                if (value == base.WantsToShoot)
                    return;
                base.WantsToShoot = value;
                if (value)
                {
                    OnStartShooting();
                }
            }
        }

        private void OnStartShooting()
        {
            shotsInSeries = 0;

            if (timeSinceLastShot > shootInterval || timeSinceLastShot < 0)
            {
                timeSinceLastShot = shootInterval;
            }
        }

        public void Update()
        {
            timeSinceLastShot += Time.deltaTime;
            bool canShoot = timeSinceLastShot >= shootInterval;
            bool seriesEnded = shotsInSeries == shootsPerTriggerHold;

            if (canShoot && WantsToShoot && !seriesEnded)
            {
                timeSinceLastShot -= shootInterval;

                shotsInSeries++;
                Shoot();
            }
        }

        public void Shoot()
        {
            Debug.Log("AAAAAAAA!");
        }
    }
}
