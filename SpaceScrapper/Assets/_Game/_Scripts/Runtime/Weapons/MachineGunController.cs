using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This controls a weapon that shoot a continues burst of bullets
    /// </summary>
    public class MachineGunController : WeaponController
    {
        [SerializeField] private float shootInterval = 0.1f;

        private float timeSinceLastShot = 0;

        public void Update()
        {
            timeSinceLastShot += Time.deltaTime;
            bool canShoot = timeSinceLastShot > shootInterval;

            if (canShoot && WantsToShoot)
            {
                timeSinceLastShot -= shootInterval;

                Shoot();
            }
        }

        public void Shoot()
        {
            Debug.Log("AAAAAAAA!");
        }
    }
}
