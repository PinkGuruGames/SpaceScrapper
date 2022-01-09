using UnityEngine.InputSystem;

namespace SpaceScrapper.Weapons.Interfaces
{
    public interface IReloadable
    {
        int MagazineSize { get; set; }
        float ReloadTime { get; set; }
        
        void Reload(InputAction.CallbackContext context);
    }
}