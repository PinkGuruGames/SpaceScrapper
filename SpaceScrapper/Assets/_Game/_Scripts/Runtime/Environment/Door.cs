using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// A very basic script for a door that can only be opened.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour, ITriggerable 
    {
        private Animator animator;

        void Start()
        {
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Set the animator trigger to cause the door to open.
        /// </summary>
        public void Open()
        {
            animator.SetTrigger("Open");
        }

        public void Trigger() => Open();
    }
}
