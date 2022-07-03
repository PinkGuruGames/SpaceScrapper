using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceScrapper
{
    /// <summary>
    /// The pause menu script specifically for the demo. This is not meant to be a good or fully functional version, just something to make things work.
    /// </summary>
    public class PauseMenu : MonoBehaviour
    {
        public InputData InputData { get; set; }

        void Awake()
        {
            Resume();
        }

        /// <summary>
        /// Just load the current scene new via its build index.
        /// </summary>
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Pause()
        {
            Time.timeScale = 0;
            gameObject.SetActive(true);
        }

        public void Resume()
        {
            Time.timeScale = 1;
            gameObject.SetActive(false);
            InputData.ActivateGameplayControls();
        }

        public void QuitCompletely()
        {
            Application.Quit();
        }

    }
}
