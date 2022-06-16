using System.Collections;
using System;
using UnityEngine;
using SpaceScrapper.Global;
using TMPro;
using UnityEngine.InputSystem;

namespace SpaceScrapper
{
    /// <summary>
    /// Shows dialogue text on screen.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        [SerializeField]
        private float durationPerWord = 1.3f;
        [SerializeField]
        private TextMeshProUGUI overlayText;
        [SerializeField]
        private CanvasGroup overlayGroup;

        [SerializeField]
        private TextMeshProUGUI fullscreenText;
        [SerializeField]
        private CanvasGroup fullscreenGroup;

        //input is required. This is a temporary measure.
        [SerializeField]
        private InputActionReference skipActionRef;
        [SerializeField]
        private InputActionReference continueActionRef;

        [SerializeField]
        private DialogueAsset testAsset;

        private DialogueAsset current;

#if UNITY_EDITOR
        private void Update()
        {
            if (current == null && testAsset != null)
                StartDialogue(testAsset, null);
        }
#endif
        private void OnEnable()
        {
            Game.SceneContext.DialogueHandler.Bind(this);
        }

        private void OnDisable()
        {
            Game.SceneContext.DialogueHandler.Unbind(this);
        }


        public void StartDialogue(DialogueAsset asset, Action onDialogueFinished)
        {
            //stop all coroutines.
            StopAllCoroutines();
            //reset any elements that may be in use.
            ResetElements();
            //set current
            current = asset;
            TextMeshProUGUI targetText;
            CanvasGroup targetGroup;
            //get the appropriate text and canvasgroup
            if(current.Type == DialogueType.Fullscreen)
            {
                targetText = fullscreenText;
                targetGroup = fullscreenGroup;
            }
            else //if (current.Type == DialogueType.Overlay)
            {
                targetGroup = overlayGroup;
                targetText = overlayText;
            }
            //start the coroutine.
            StartCoroutine(Co_ShowText(targetText, targetGroup, onDialogueFinished));
        }

        /// <summary>
        /// Reset the text elements and their canvas groups.
        /// </summary>
        private void ResetElements()
        {
            overlayText.text = "";
            overlayGroup.alpha = 0;
            fullscreenText.text = "";
            fullscreenGroup.alpha = 0;
        }

        /// <summary>
        /// Coroutine that displays the current dialogue text on the correct text field.
        /// </summary>
        private IEnumerator Co_ShowText(TextMeshProUGUI targetText, CanvasGroup targetGroup, Action onDialogueFinished)
        {
            //fade in the text group.
            for(float t = 0; t <= 1; t += Time.deltaTime)
            {
                targetGroup.alpha = t;
                yield return null;
            }
            targetGroup.alpha = 1;
            //display all the text
            for (int i = 0; i < current.Lines.Length; i++)
            {
                float wait = durationPerWord * (CountSpacesInString(current.Lines[i]) + 1);

                targetText.text = current.Lines[i];

                yield return new WaitForSeconds(wait);
            }
            onDialogueFinished?.Invoke();
            yield return null;
            //fade out the text group.
            for (float t = 1; t > 0; t -= Time.deltaTime)
            {
                targetGroup.alpha = t;
                yield return null;
            }
            targetGroup.alpha = 0;
        }

        /// <summary>
        /// Counts the amount of unique visible spaces in a string. 
        /// e.g. "a b c" => 2 || "a   c" => 1 || " a " => 2
        /// </summary>
        private int CountSpacesInString(string s)
        {
            int spaces = (s[0] == ' ') ? 1 : 0;
            for (int i = 1; i < s.Length; i++)
                if (s[i] == ' ' && s[i - 1] != ' ')
                    spaces++;
            return spaces;
        }

    }
}