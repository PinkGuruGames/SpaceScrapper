using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceScrapper
{
    /// <summary>
    /// ScriptableObject type asset that defines a dialogue.
    /// </summary>
    [CreateAssetMenu(menuName = "Custom Data/Dialogue Asset", fileName = "new Dialogue")]
    public class DialogueAsset : ScriptableObject
    {
        [SerializeField]
        private DialogueType type;
        [SerializeField]
        private string[] lines;

        public DialogueType Type => type;
        public string[] Lines => lines;
    }

    /// <summary>
    /// Based on the Dialogue Type, a dialogue is treated differently by the in-scene DialogueManager.
    /// </summary>
    public enum DialogueType
    {
        //default
        Fullscreen = 0,
        Overlay = 1
    }
}
