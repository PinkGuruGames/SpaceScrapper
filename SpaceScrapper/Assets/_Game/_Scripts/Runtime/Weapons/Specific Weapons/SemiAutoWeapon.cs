using SpaceScrapper.Weapons;
using SpaceScrapper.Weapons.Interfaces;
using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper
{
    public class SemiAutoWeapon : Weapon // Might change the name of the class
    {
        private float _cooldownTime;
        
        [SerializeField] private float cooldown;

        private void Update()
        {
            if (_cooldownTime > 0)
                _cooldownTime -= Time.deltaTime;
        }

        protected internal override void ToggleShooting(InputAction.CallbackContext context)
        {
            if(_cooldownTime <= 0)
                Shoot();
        }
    }
}
