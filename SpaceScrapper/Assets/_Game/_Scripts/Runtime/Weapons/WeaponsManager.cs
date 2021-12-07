using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This class manages weapons the player has available and passes the input to them 
    /// </summary>
    public class WeaponsManager : MonoBehaviour
    {
        [SerializeField] private List<WeaponController> weapons;

        private int currentWeaponIndex = 0;

        private WeaponController CurrentWeapon
        {
            get
            {
                if (currentWeaponIndex < 0 || currentWeaponIndex >= weapons.Count)
                    return null;
                return weapons[currentWeaponIndex];
            }
        }

        public void Update()
        {
            // HACK: Input should be handled by the player controller and not be handled internally, change later
            for (int i = 0; i < 8; i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    SwitchWeaponTo(i);
                }
            }

            SetContinuousShooting(Input.GetKey(KeyCode.P));
        }

        public void SwitchWeaponTo(int i)
        {
            if (CurrentWeapon != null)
                CurrentWeapon.WantsToShoot = false;

            currentWeaponIndex = i;
        }

        public void SetContinuousShooting(bool v)
        {
            if (CurrentWeapon == null)
                return;

            CurrentWeapon.WantsToShoot = v;
        }
    }
}
