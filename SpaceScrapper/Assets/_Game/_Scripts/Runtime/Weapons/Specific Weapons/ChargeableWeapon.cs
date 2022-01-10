using System.Collections;
using SpaceScrapper.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceScrapper._Game._Scripts.Runtime.Weapons.Specific_Weapons
{
    public class ChargeableWeapon : Weapon
    {
        private Coroutine _chargingCoroutine;
        private float _chargingStatus;

        [SerializeField] private int chargingLimit;
        
        protected internal override void ToggleShooting(InputAction.CallbackContext context)
        {
            if (_chargingCoroutine is null)
                _chargingCoroutine = StartCoroutine(Co_Charge());
            else
            {
                StopCoroutine(_chargingCoroutine);
                _chargingCoroutine = null;
            }
        }

        protected override void Shoot()
        {
            base.Shoot();
            Debug.Log("Charged!");
            // Shoot
        }

        private IEnumerator Co_Charge()
        {
            while (_chargingStatus < chargingLimit)
            {
                _chargingStatus += Time.deltaTime;
                // Update UI or any other fancy effects
                yield return null;
            }

            Shoot();
        }
    }
}