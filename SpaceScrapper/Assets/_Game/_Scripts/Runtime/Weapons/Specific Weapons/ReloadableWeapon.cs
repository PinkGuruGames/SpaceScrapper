using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public class ReloadableWeapon : Weapon
    {
        private int _currentAmmo;
        private Coroutine _reloadingCoroutine;

        [SerializeField] private int currentReserveAmmo;
        [SerializeField] private int magazineSize;
        [SerializeField] private float reloadTime;
        
        protected Coroutine ShootingCoroutine { get; set; }
        protected bool ShootAfterReload { get; set; }

        protected override void Start()
        {
            base.Start();
            _currentAmmo = magazineSize;
        }

        protected internal void Reload()
        {
            if (currentReserveAmmo <= 0 || _reloadingCoroutine is not null)
            {
                // UI-Feedback, Sound Effects, other fancy things
                return;
            }

            if (ShootingCoroutine is not null)
            {
                StopCoroutine(ShootingCoroutine);
                ShootingCoroutine = null;
                ShootAfterReload = true;
            }

            _reloadingCoroutine = StartCoroutine(Co_Reload());
        }

        protected override void Shoot()
        {
            if (_currentAmmo <= 0)
            {
                Reload(); // TODO: only if auto-reload enabled in settings
                return;
            }
            _currentAmmo--;
            
            base.Shoot();
        }
        
        private IEnumerator Co_Reload()
        {
            Debug.Log("Reload");
            yield return new WaitForSeconds(reloadTime);
            // Sound-Effects
            // Other fancy things
            
            if (currentReserveAmmo >= magazineSize)
            {
                _currentAmmo = magazineSize;
                currentReserveAmmo -= magazineSize;
            }
            else
            {
                _currentAmmo = currentReserveAmmo;
                currentReserveAmmo = 0;
                // Specific UI-Feedback
            }
            
            // UI-Feedback

            Debug.Log("Reload done!");
            _reloadingCoroutine = null;
        }

        protected internal override void ToggleShooting()
        {
            //idk
        }
    }
}