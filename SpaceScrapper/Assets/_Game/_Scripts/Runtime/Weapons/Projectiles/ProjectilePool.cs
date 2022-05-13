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

        private ProjectileBase[] pooledProjectiles;
        private int storedProjectiles;
        private int projectileInstances;

        private void Awake()
        {
            dynamicPoolMaxSize = Mathf.Max(minimumPoolSize, dynamicPoolMaxSize); //make sure that the max isnt lower than the minimum
            pooledProjectiles = new ProjectileBase[dynamicPoolMaxSize];
            //SceneManager.sceneUnloaded += InitializePool;
            Debug.Log("Awake " + name);
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable " + name); 
        }

        private void InitializePool()
        {
            pooledProjectiles = new ProjectileBase[dynamicPoolMaxSize];
            for(int i = 0; i < minimumPoolSize; i++)
            {
                pooledProjectiles[i] = CreateProjectileInstance();
            }
        }

        /// <summary>
        /// Get a projectile from the pool if possible.
        /// </summary>
        /// <returns></returns>
        public ProjectileBase Get()
        {
            if (storedProjectiles > 0)
            {
                return pooledProjectiles[storedProjectiles--];
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
