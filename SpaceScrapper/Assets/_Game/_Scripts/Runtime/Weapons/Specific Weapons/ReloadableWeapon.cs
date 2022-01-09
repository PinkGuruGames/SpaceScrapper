using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public class ReloadableWeapon : Weapon
    {
        private Coroutine _reloadingCoroutine;

        [SerializeField] private int currentReserveAmmo;
        [SerializeField] private int magazineSize;
        [SerializeField] private float reloadTime;
        
        protected Coroutine ShootingCoroutine { get; set; }
        protected bool ShootAfterReload { get; set; }
        protected int CurrentAmmo { get; set; }
        
        private void Start()
        {
            CurrentAmmo = magazineSize;
        }

        protected internal void Reload(InputAction.CallbackContext context)
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
        
        private IEnumerator Co_Reload()
        {
            Debug.Log("Reload");
            yield return new WaitForSeconds(reloadTime);
            // Sound-Effects
            // Other fancy things
            
            if (currentReserveAmmo >= magazineSize)
            {
                CurrentAmmo = magazineSize;
                currentReserveAmmo -= magazineSize;
            }
            else
            {
                CurrentAmmo = currentReserveAmmo;
                currentReserveAmmo = 0;
                // Specific UI-Feedback
            }
            
            // UI-Feedback

            Debug.Log("Reload done!");
            _reloadingCoroutine = null;
        }
    }
}