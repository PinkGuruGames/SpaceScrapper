using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpaceScrapper.Levels
{
    public class LevelEntrance : MonoBehaviour
    {
        [SerializeField]
        private LevelInfo levelInfo;

        [SerializeField]
        private Collider2D innerRing, outerRing;

        [SerializeField]
        private TextMeshProUGUI nameDisplay;

        private bool showingRealName = false;

        // Start is called before the first frame update
        void Start()
        {
            //start out with a scrambled name.
            nameDisplay.text = GetRandomLetters(levelInfo.LevelName.Length);
        }

        private void OnEnable()
        {
            levelInfo.Entrance.Bind(this);
        }

        private void OnDisable()
        {
            levelInfo.Entrance.Unbind(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(showingRealName == false)
            {
                //reveal the name upon the first time of the player approaching the level.
                showingRealName = true;
                StartCoroutine(RevealName());
            }
        }

        //coroutine for revealing the "real" name of the level.
        private IEnumerator RevealName()
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
    }
}
