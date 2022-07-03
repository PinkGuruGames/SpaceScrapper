using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceScrapper.Weapons
{
    /// <summary>
    /// A pool for projectiles.
    /// </summary>
    [CreateAssetMenu(fileName = "new Pool", menuName = "Custom Data/Projectile Pool")]
    public class ProjectilePool : ScriptableObject
    {
        [SerializeField]
        private int minimumPoolSize = 100;
        [SerializeField]
        private int dynamicPoolMaxSize = 500;
        [SerializeField]
        private GameObject projectilePrefab;

        private ProjectileBase[] pooledProjectiles = null;
        private int storedProjectiles = 0;
        private int projectileInstances = 0;

        /// <summary>
        /// Used by Weapons to make sure that their assigned projectile pool is initialized.
        /// </summary>
        internal void Initialize()
        {
            //make sure that its not initialized already!
            //conditions for being already initialized (in the current scene)
            //- projectile array pooledProjectiles does not exist yet
            //- projectiles should be stored, but first entry is null (destroyed)
            Debug.Log("trying to initialize pool");
            if(pooledProjectiles == null || (storedProjectiles > 0 && pooledProjectiles[0] == null) || pooledProjectiles.Length == 0 || storedProjectiles == 0)
            {
                InitializePool();
                Debug.Log("initialize: success");
            }
        }

        /// <summary>
        /// Initializes the pool with the minimum amount of projectiles.
        /// </summary>
        private void InitializePool()
        {
#if UNITY_EDITOR
            if (Application.isEditor && Application.isPlaying == false)
                return;
#endif
            dynamicPoolMaxSize = Mathf.Max(minimumPoolSize, dynamicPoolMaxSize); //make sure that the max isnt lower than the minimum
            projectileInstances = 0;
            pooledProjectiles = new ProjectileBase[dynamicPoolMaxSize];
            for(int i = 0; i < minimumPoolSize; i++)
            {
                pooledProjectiles[i] = CreateProjectileInstance();
            }
            storedProjectiles = minimumPoolSize;
        }

        /// <summary>
        /// Get a projectile from the pool if possible.
        /// </summary>
        /// <returns></returns>
        public ProjectileBase Get()
        {
            if (storedProjectiles > 0)
            {
                storedProjectiles--;
                return pooledProjectiles[storedProjectiles];
            }
            else if (projectileInstances < dynamicPoolMaxSize)
            {
                return CreateProjectileInstance();
            }
            Debug.LogError("Trying to get more projectiles than the set maximum limit.", this);
            return null;
        }

        /// <summary>
        /// Create a new instance of the projectile.
        /// </summary>
        /// <returns>The created instance.</returns>
        private ProjectileBase CreateProjectileInstance()
        {
            ProjectileBase proj = Instantiate(projectilePrefab).GetComponent<ProjectileBase>();
            proj.Pool = this;
            projectileInstances++;
            proj.Initialize();
            proj.gameObject.SetActive(false);
            return proj;
        }

        /// <summary>
        /// Insert a projectile back into its pool.
        /// </summary>
        /// <param name="projectile">A projectile that belongs to the pool.</param>
        public void Insert(ProjectileBase projectile)
        {
            //check if it belongs in here.
            if(projectile.Pool == this)
            {
                pooledProjectiles[storedProjectiles] = projectile;
                storedProjectiles++;
            }
            else
            {
                Debug.LogWarning("Trying to insert a projectile in a different pool than it was assigned to. This is not okay.");
            }
        }
    }
}
