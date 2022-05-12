using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] protected float TimeBetweenShots { get; set; }

        protected internal virtual void ToggleShooting(InputAction.CallbackContext context) { }
        
        protected virtual void Shoot()
        {
            Debug.Log("Pew Pew!");
            // Sound etc.
        }
    }
}
