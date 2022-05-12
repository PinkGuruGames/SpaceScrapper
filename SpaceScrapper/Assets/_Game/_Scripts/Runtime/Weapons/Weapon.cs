using UnityEngine;
using UnityEngine.InputSystem;

// ReSharper disable CheckNamespace
namespace SpaceScrapper.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [field: SerializeField] protected float TimeBetweenShots { get; set; }

        [SerializeField]
        private GameObject projectilePrefab; //TESTING ONLY

        protected internal abstract void ToggleShooting();
        
        protected virtual void Shoot()
        {
            Debug.Log("Pew Pew!");
            // Sound etc.
            ProjectileBase projectile = Instantiate(projectilePrefab).GetComponent<ProjectileBase>();
            projectile.FireWithParameters(null, transform.position, transform.up, 1); //THIS IS ONLY FOR TESTING YEAH!
        }
    }
}
