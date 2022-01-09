using System.Collections;
using System.Diagnostics.CodeAnalysis;
using SpaceScrapper.Weapons.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    [SuppressMessage("ReSharper", "IteratorNeverReturns")]
    public class AutomaticWeapon : Weapon, IReloadable
    {
        private int _currentAmmo;
        private Coroutine _shootingCoroutine;
        private Coroutine _reloadingCoroutine;
        private bool _shootAfterReload;

        [SerializeField] private int currentReserveAmmo;
        
        [field: SerializeField] public int MagazineSize { get; set; }
        [field: SerializeField] public float ReloadTime { get; set; }

        private void Start()
        {
            _currentAmmo = MagazineSize;
        }

        public void Reload(InputAction.CallbackContext context)
        {
            if (currentReserveAmmo <= 0 || _reloadingCoroutine is not null)
            {
                // UI-Feedback, Sound Effects, other fancy things
                return;
            }

            if (_shootingCoroutine is not null)
            {
                StopCoroutine(_shootingCoroutine);
                _shootingCoroutine = null;
                _shootAfterReload = true;
            }

            _reloadingCoroutine = StartCoroutine(Co_Reload());
        }
        
        protected internal override void ToggleShooting(InputAction.CallbackContext context)
        {
            if (_shootAfterReload)
            {
                _shootAfterReload = false;
                return;
            }
            
            if(_shootingCoroutine is null)
                _shootingCoroutine = StartCoroutine(Co_Shoot());
            else
            {
                StopCoroutine(_shootingCoroutine);
                _shootingCoroutine = null;
            }
        }

        protected override void Shoot()
        {
            if (_currentAmmo <= 0)
            {
                Reload(new InputAction.CallbackContext()); // TODO: only if auto-reload enabled in settings
                return;
            }
            
            base.Shoot();
            Debug.Log("Pew pew (automatic weapon)!!!");
            _currentAmmo--;
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

        private IEnumerator Co_Reload()
        {
            Debug.Log("Reload");
            yield return new WaitForSeconds(ReloadTime);
            // Sound-Effects
            // Other fancy things
            
            if (currentReserveAmmo >= MagazineSize)
            {
                _currentAmmo = MagazineSize;
                currentReserveAmmo -= MagazineSize;
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
    }
}
