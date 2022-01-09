using SpaceScrapper.Weapons;
using SpaceScrapper.Weapons.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper
{
    public class SemiAutoWeapon : Weapon // Might change the name of the class
    {
        private float _cooldown;

        private void Update()
        {
            if (_cooldown > 0)
                _cooldown -= Time.deltaTime;
        }

        protected internal override void ToggleShooting(InputAction.CallbackContext context)
        {
            if (_cooldown > 0)
                return;
            
            Shoot();
            _cooldown = TimeBetweenShots;
        }
    }
}