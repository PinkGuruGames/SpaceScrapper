using System.Collections;
using SpaceScrapper.Weapons.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceScrapper.Weapons
{
    public class AutomaticWeapon : Weapon, IReloadable
    {
        private int _currentReserveAmmo;
        private int _currentAmmo;
        private Coroutine _shootingCoroutine;

        [SerializeField] private float timeBetweenShots;
        
        [field: SerializeField] public int MagazineSize { get; set; }
        [field: SerializeField] public float ReloadTime { get; set; }

        public void Reload(InputAction.CallbackContext context)
        {
            if (_currentReserveAmmo <= 0)
            {
                // UI-Feedback, Sound Effects, other fancy things
                return;
            }

            StartCoroutine(Co_Reload());
        }

        protected override void Shoot()
        {
            base.Shoot();

            Debug.Log("Pew pew (automatic weapon)!!!");
            
            if (_currentAmmo <= 0) // and auto-reload set in settings
                Reload(new InputAction.CallbackContext());
            else
                _currentAmmo--;
        }

        protected internal override void ToggleShooting(InputAction.CallbackContext context)
        {
            if(_shootingCoroutine is null)
                _shootingCoroutine = StartCoroutine(Co_Shoot());
            else
            {
                StopCoroutine(_shootingCoroutine);
                _shootingCoroutine = null;
            }
        }

        private IEnumerator Co_Reload()
        {
            yield return new WaitForSeconds(ReloadTime);
            // Sound-Effects
            // Other fancy things
            
            if (_currentReserveAmmo >= MagazineSize)
            {
                _currentReserveAmmo -= MagazineSize;
                _currentAmmo = MagazineSize;
                // UI-Feedback
            }
            else
            {
                _currentAmmo = _currentReserveAmmo;
                _currentReserveAmmo = 0;
                // UI-Feedback
            }
        }

        private IEnumerator Co_Shoot()
        {
            var wait = new WaitForSeconds(timeBetweenShots);
            while (true)
            {
                Shoot();
                yield return wait;
            }
        }
    }
}
