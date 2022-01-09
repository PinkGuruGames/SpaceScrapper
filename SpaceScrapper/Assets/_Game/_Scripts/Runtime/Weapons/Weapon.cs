using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] protected float TimeBetweenShots { get; set; }
        
        protected internal abstract void ToggleShooting(InputAction.CallbackContext context);
        
        protected virtual void Shoot()
        {
            // Sound etc.
        }
    }
}
