using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceScrapper.Weapons
{
    public class Gun : Weapon
    {
        protected internal override void Shoot(InputAction.CallbackContext context)
        {
            Debug.Log("Shoot (Gun)!");
        }
    }
}
