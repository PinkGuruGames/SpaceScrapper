using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected internal abstract void ToggleShooting(InputAction.CallbackContext context);
        
        protected virtual void Shoot()
        {
            // Sound etc.
        }
    }
}
