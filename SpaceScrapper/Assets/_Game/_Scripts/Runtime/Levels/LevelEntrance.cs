using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SpaceScrapper.Global;

namespace SpaceScrapper.Levels
{
    /// <summary>
    /// The in-world functionality of the entrance to a level.
    /// </summary>
    public class LevelEntrance : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private LevelInfo levelInfo;

        [SerializeField]
        private Collider2D innerRing, outerRing;

        [SerializeField]
        private TextMeshProUGUI nameDisplay;

        private bool showingRealName = false;
        private bool insideInnerRing = false;

        // Start is called before the first frame update
        void Start()
        {
            //start out with a scrambled name?
            nameDisplay.text = showingRealName? levelInfo.LevelName : GetRandomLetters(levelInfo.LevelName.Length);
        }

        private void OnEnable()
        {
            //bind the entrance in OnEnable, //note: might have to be moved to Awake instead.
            levelInfo.Entrance.Bind(this);
        }

        private void OnDisable()
        {
            levelInfo.Entrance.Unbind(this);
        }

        /// <summary>
        /// Check for player entering the triggers on this object.
        /// </summary>
        /// <param name="other"></param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(showingRealName == false)
            {
                //reveal the name upon the first time of the player approaching the level.
                showingRealName = true;
                StartCoroutine(Co_RevealName());
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            bool insideInner = other.IsTouching(innerRing);
            if (insideInner is true && insideInnerRing is false)
            {
                insideInnerRing = true;
                PlayerInteractor.RegisterInteractable(this);
            }
            else if(insideInner is false && insideInnerRing is true)
            {
                insideInnerRing = false;
                PlayerInteractor.UnregisterInteractable(this);
            }
        }

        //coroutine for revealing the "real" name of the level.
        private IEnumerator Co_RevealName()
        {
            //totalDuration/nameLength = time per letter.
            float timePerLetter = 1.2f / (float)levelInfo.LevelName.Length;
            var wait = new WaitForSeconds(timePerLetter);
            //reveal one letter at a time.
            for (int i = 0; i <= levelInfo.LevelName.Length; i++)
            {
                //string + is kinda not the nicest, but it does the job for now.
                string text = levelInfo.LevelName.Substring(0, i) + GetRandomLetters(levelInfo.LevelName.Length - i);
                nameDisplay.text = text;
                yield return wait;
            }
        }

        //Get a string of [count] random letters from a selection of predefined letters (constant)
        private string GetRandomLetters(int count)
        {
            string val = "";
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ$%&=?#1234567890";
            for (int i = 0; i < count; i++)
            {
                val += letters[Random.Range(0, letters.Length)];
            }
            return val;
        }

        public void Interact(PlayerInteractor interactor)
        {
            //Load Level
            GameSceneManager.LoadSceneByName(levelInfo.SceneName);
            //setup level context info.
            Game.SceneContext.CurrentLevel.Bind(levelInfo);
            //throw new System.NotImplementedException("Level should be loading, but it hasnt been set up yet. - LevelEntrance");
        }

        /// <summary>
        /// WARNING: This should only be called by the player, AFTER the player object was placed at this transforms position via the LevelInfo SharedHandle.
        /// </summary>
        public void UnbindLevelInfoFromContext()
        {
            Game.SceneContext.CurrentLevel.Unbind(levelInfo);
        }
    }
}
