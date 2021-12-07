using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// This is a base class for a weapon. It defines common signature and functionality of every weapon
    /// </summary>
    // Non ideal implementation? Might be replaced by strategy pattern later
    public abstract class WeaponController : MonoBehaviour
    {
        public virtual bool WantsToShoot { set; get; } // Should this have a getter?
    }
}
