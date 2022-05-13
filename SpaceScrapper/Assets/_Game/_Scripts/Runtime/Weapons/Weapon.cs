using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] protected float TimeBetweenShots { get; set; }

        [SerializeField]
        private ProjectilePool projectilePool; //TESTING ONLY

        protected internal abstract void ToggleShooting();
        
        protected virtual void Start()
        {
            projectilePool.Initialize();
        }

        protected virtual void Shoot()
        {
            Debug.Log("Pew Pew!");
            // Sound etc.
            ProjectileBase projectile = projectilePool.Get();
            projectile.FireWithParameters(null, transform.position, transform.up, 1); //THIS IS ONLY FOR TESTING YEAH!
        }
    }
}
