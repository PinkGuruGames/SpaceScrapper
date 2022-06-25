using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    /// <summary>
    /// Controller for Weapons that are used by the player.
    /// Gets input events via PlayerInput.
    /// </summary>
    public class WeaponController : MonoBehaviour
    {
        //private MainInput _mainInput;
        private Weapon _currentWeapon;

        [SerializeField] 
        private GameObject weaponPrefab; // Automatically set in code
        [SerializeField] 
        private Transform weaponParent;
        [SerializeField]
        private Weapon testWeapon;

        public bool ActiveWeapons { get; set; } = true;

        private void Awake()
        {
            InitWeapon();
        }

        /// <summary>
        /// Instantiate the weapon prefab for now.
        /// </summary>
        private void InitWeapon()
        {
            //_currentWeapon = Instantiate(weaponPrefab, weaponParent).GetComponent<Weapon>();
            _currentWeapon = testWeapon;
        }

        //Input Events from PlayerInput
        private void OnReload()
        {
            if( _currentWeapon != null && _currentWeapon is ReloadableWeapon wp)
            {
                wp.Reload();
            }
        }

        private void OnFirePrimary(InputValue value)
        {
            if(ActiveWeapons && _currentWeapon)
                _currentWeapon.ToggleShooting();
        }

        private void OnFireSecondary()
        {

        }
    }
}
