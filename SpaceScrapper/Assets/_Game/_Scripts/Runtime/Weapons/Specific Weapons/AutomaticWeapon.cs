using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    public class AutomaticWeapon : ReloadableWeapon
    {
        /// <summary>
        /// The time (Time.time) when the weapon has fired the last time.
        /// </summary>
        private float lastShotFired = 0f;

        protected internal override void ToggleShooting()
        {
            if (ShootAfterReload)
            {
                ShootAfterReload = false;
                return;
            }
            
            //either turn on or turn off shooting.
            if(ShootingCoroutine is null)
                ShootingCoroutine = StartCoroutine(Co_Shoot());
            else
            {
                StopCoroutine(ShootingCoroutine);
                ShootingCoroutine = null;
            }
        }

        /// <summary>
        /// Coroutine that handles automatic shooting.
        /// </summary>
        private IEnumerator Co_Shoot()
        {
            // Time between shots too short check: 
            // next possible shot time - current time 
            // negative difference = okay to shoot
            // positive difference = wait 
            float timeOffset = (lastShotFired + TimeBetweenShots) - Time.time;
            if (timeOffset > 0)
            {
                yield return new WaitForSeconds(timeOffset);
            }

            //the standard shooting loop.
            var wait = new WaitForSeconds(TimeBetweenShots);
            while (true)
            {
                Shoot();
                lastShotFired = Time.time;
                yield return wait;
            }
        }
    }
}
