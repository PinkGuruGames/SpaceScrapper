using System;
using UnityEngine;
using UnityEngine.Events;
using SpaceScrapper.Global;

namespace SpaceScrapper.DialogueSystem
{
    /// <summary>
    /// Simple script that starts a predefined dialogue, and causes something to happen afterwards.
    /// </summary>
    [AddComponentMenu("Triggerable/DialogueStarter")]
    public class DialogueStarter : MonoBehaviour, ITriggerable
    {
        [SerializeField]
        private DialogueAsset dialogueAsset;
        [SerializeField]
        private UnityEvent onDialogueEnded;

        public void Trigger()
        {
            Game.SceneContext.DialogueHandler.Value.StartDialogue(dialogueAsset, onDialogueEnded.Invoke);
        }
    }
}
