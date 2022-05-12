using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable once CheckNamespace
namespace SpaceScrapper.Weapons
{
    public class ChargeableWeapon : Weapon
    {
        private Coroutine _chargingCoroutine;
        private float _chargingStatus;
        private bool _charged;
        private bool _shooting;

        [SerializeField] private int chargingLimit;

        protected internal override void ToggleShooting()
        {
            if (_charged)
            {
                _charged = false;
                return;
            }
            
            if (_chargingCoroutine is null)
                _chargingCoroutine = StartCoroutine(Co_Charge());
            else
            {
                _chargingStatus = 0;
                StopCoroutine(_chargingCoroutine);
                _chargingCoroutine = null;
            }
        }

        protected override void Shoot()
        {
            base.Shoot();
            Debug.Log("Charged and ready to shoot!");
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

            Debug.Log("Charged!");
            _chargingStatus = 0;
            if(_shooting)
                _charged = true;
            Shoot();
        }
    }
}