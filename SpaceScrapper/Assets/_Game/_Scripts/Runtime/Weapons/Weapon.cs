using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected virtual void Shoot()
        {
            // Sound etc.
        }
        
        protected internal abstract void ToggleShooting(InputAction.CallbackContext context);
    }
}
