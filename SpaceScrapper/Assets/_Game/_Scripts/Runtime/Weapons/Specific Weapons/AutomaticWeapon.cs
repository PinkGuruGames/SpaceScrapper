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
        protected internal override void ToggleShooting(InputAction.CallbackContext context)
        {
            if (ShootAfterReload)
            {
                ShootAfterReload = false;
                return;
            }
            
            if(ShootingCoroutine is null)
                ShootingCoroutine = StartCoroutine(Co_Shoot());
            else
            {
                StopCoroutine(ShootingCoroutine);
                ShootingCoroutine = null;
            }
        }

        protected override void Shoot()
        {
            if (CurrentAmmo <= 0)
            {
                Reload(new InputAction.CallbackContext()); // TODO: only if auto-reload enabled in settings
                return;
            }
            
            base.Shoot();
            CurrentAmmo--;
        }

        private IEnumerator Co_Shoot()
        {
            var wait = new WaitForSeconds(TimeBetweenShots);
            while (true)
            {
                Shoot();
                yield return wait;
            }
        }
    }
}
