using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpaceScrapper
{
    public class LevelEntrance : MonoBehaviour
    {
        [SerializeField]
        private string levelName;

        [SerializeField]
        private Collider2D innerRing, outerRing;

        [SerializeField]
        private TextMeshProUGUI nameDisplay;

        private bool showingRealName = false;

        // Start is called before the first frame update
        void Start()
        {
            //start out with a scrambled name.
            nameDisplay.text = GetRandomLetters(levelName.Length);
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
            float timePerLetter = 1.2f / (float)levelName.Length;
            var wait = new WaitForSeconds(timePerLetter);
            //reveal one letter at a time.
            for (int i = 0; i <= levelName.Length; i++)
            {
                //string + is kinda not the nicest, but it does the job for now.
                string text = levelName.Substring(0, i) + GetRandomLetters(levelName.Length - i);
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
