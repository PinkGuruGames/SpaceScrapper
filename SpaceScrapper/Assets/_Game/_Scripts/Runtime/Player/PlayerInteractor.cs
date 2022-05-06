using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A small script that only exists to handle "interactions" of the player with other objects.
    /// </summary>
    public class PlayerInteractor : MonoBehaviour
    {
        private static PlayerInteractor instance;

        private List<IInteractable> interactables = new List<IInteractable>(5);

        private int selectedIndex = 0;

        //Setup in awake
        private void Awake()
        {
            instance = this;
        }

        private void OnDestroy()
        {
            instance = null;
        }

        /// <summary>
        /// Message sent by PlayerInput when the interact InputAction is triggered.
        /// </summary>
        private void OnInteract()
        {
            if(interactables.Count > 0)
            {
                interactables[selectedIndex].Interact(this);
            }
        }

        public static void RegisterInteractable(IInteractable interactable)
        {
            if(instance)
                instance.Register(interactable);
        }

        private void Register(IInteractable interactable)
        {
            if(interactables.Count == 0)
            {
                //Show prompt for interacting
            }
            else if(interactables.Count == 1)
            {
                //show additional prompt for switching selection
            }
            interactables.Add(interactable);
        }

        public static void UnregisterInteractable(IInteractable interactable)
        {
            if (instance)
                instance.Unregister(interactable);
        }

        private void Unregister(IInteractable interactable)
        {
            if(interactables.Count == 1)
            {
                //hide prompt for interacting
            }
            else if(interactables.Count == 2)
            {
                //hide additional prompt for switching selection
            }
            interactables.Remove(interactable);
            selectedIndex = Mathf.Clamp(selectedIndex, 0, interactables.Count);
        }
    }
}
