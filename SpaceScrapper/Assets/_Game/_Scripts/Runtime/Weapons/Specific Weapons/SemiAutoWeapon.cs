using SpaceScrapper.Weapons;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper
{
    public class SemiAutoWeapon : ReloadableWeapon // Might change the name of the class
    {
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
            
            Shoot();
            _cooldown = TimeBetweenShots;
        }
    }
}
