using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// THis class controls a weapon that shoots once per key click
    /// </summary>
    public class SingleShotWeaponController : WeaponController
    {
        [SerializeField] private float shootCooldown = 0.1f;

        private float cooldown = 0;

        public override bool WantsToShoot
        {
            set
            {
                if (value && !base.WantsToShoot)
                    TryToShoot();

                base.WantsToShoot = value;
            }
        }

        public void Update()
        {
            if (cooldown > 0)
                cooldown -= Time.deltaTime;
        }

        private void TryToShoot()
        {
            if (cooldown > 0)
                return;

            Shoot();
            cooldown = shootCooldown;
        }

        public void Shoot()
        {
            Debug.Log("AAAAAAAA!");
        }
    }
}
