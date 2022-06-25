using SpaceScrapper.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

// ReSharper disable CheckNamespace
namespace SpaceScrapper
{
    public class SemiAutoWeapon : ReloadableWeapon // Might change the name of the class
    {
        //adding burst in here because it didnt make sense to make a seperate class for it,
        //and inheritance from this is weird with the ToggleShooting override.
        [SerializeField, Range(1, 5)] 
        private int burstSize = 1;
        [SerializeField, Range(0.01f, 1f)]
        private float burstInterval;

        private float _cooldown;

        private void Update()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }

        protected internal override void ToggleShooting()
        {
            if (_cooldown > 0)
                return;
            if (burstSize is 1)
            {
                Shoot();
            }
            else
            {
                StartCoroutine(Co_BurstShoot());
            }
            _cooldown = TimeBetweenShots;
        }

        private IEnumerator Co_BurstShoot()
        {
            for(int i = 0; i < burstSize; i++)
            {
                Shoot();
                yield return new WaitForSeconds(burstInterval);
            }
        }
    }
}
