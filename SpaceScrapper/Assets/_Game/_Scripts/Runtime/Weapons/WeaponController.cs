using UnityEngine;

namespace SpaceScrapper.Weapons
{
    /// <summary>
    /// Controller for the entire system revolving around weapons and shooting.
    /// Responsible for input, choosing the weapon etc.
    /// </summary>
    public class WeaponController : MonoBehaviour
    {
        private MainInput _mainInput;
        private Weapon _currentWeapon;

        [SerializeField] private GameObject weaponObject;

        private void Awake()
        {
            _mainInput = new MainInput(); // TODO: Remove this (only for testing purposes), probably global input instance
            _currentWeapon = weaponObject.AddComponent<Gun>();
        }

        private void OnEnable()
        {
            _mainInput.Enable();
            _mainInput.Weapons.Fire.started += _currentWeapon.Shoot;
        }

        private void OnDisable()
        {
            _mainInput.Disable();
            _mainInput.Weapons.Fire.started -= _currentWeapon.Shoot;
        }
    }
}
