using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    // public enum FireType
    // {
    //     SemiAutomatic, // One shot per click, pull the trigger again for another shot
    //     Automatic, // Will shoot in the specified fire-rate while the trigger is held down
    //     Burst, // Shoot several bullets per shot
    //     Continuous, // Shoot a continuous shot while the trigger is held down (like holding a laser pointer for example)
    // }
    
    public abstract class Weapon : MonoBehaviour
    {
        // protected FireType WeaponFireType { get; set; }
        [field: SerializeField] protected float FireTime { get; set; }
        
        protected internal abstract void Shoot(InputAction.CallbackContext context);
    }
}
