using UnityEngine;
using UnityEngine.InputSystem;

namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        protected internal abstract void Shoot(InputAction.CallbackContext context);
    }
}
