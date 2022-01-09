using UnityEngine;

// ReSharper disable CheckNamespace
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

        [SerializeField] private GameObject weaponPrefab; // Automatically set in code
        [SerializeField] private Transform weaponParent;

        private void Awake()
        {
            _mainInput = new MainInput(); // TODO: Remove this (only for testing purposes), probably global input instance
            InitWeapon();
        }

        private void OnEnable()
        {
            _mainInput.Enable();
            _mainInput.Weapons.Fire.started += _currentWeapon.Shoot;
            if (_currentWeapon is AutomaticWeapon)
                _mainInput.Weapons.Fire.performed += _currentWeapon.Shoot; // Maybe change subscribed method
        }

        private void OnDisable()
        {
            _mainInput.Disable();
            _mainInput.Weapons.Fire.started -= _currentWeapon.Shoot;
            _mainInput.Weapons.Fire.performed -= _currentWeapon.Shoot; // Maybe change subscribed method
        }

        private void InitWeapon()
        {
            _currentWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
        }
    }
}
