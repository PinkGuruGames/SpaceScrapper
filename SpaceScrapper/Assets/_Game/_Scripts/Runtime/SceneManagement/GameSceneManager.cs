using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SpaceScrapper
{
    /// <summary>
    /// The class responsible for loading scenes properly and updating the game context.
    /// </summary>
    public class GameSceneManager : MonoBehaviour
    {
        private static GameSceneManager instance;

        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private float fadeTime = 1.5f;

        private string currentScene;

        //basic singleton-esque setup.
        void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
                return;
            }
            DontDestroyOnLoad(gameObject);
            currentScene = SceneManager.GetActiveScene().name;
        }

        //clean up the instance reference if necessary.
        private void OnDestroy()
        {
            if(instance == this)
                instance = null;
        }

        ///<summary>private call for loading the scene, more convenient than doing instance.StartCoroutine(instance....</summary>
        private void StartLoadingSceneByName(string targetScene)
        {
            StartCoroutine(Co_LoadSceneByName(targetScene));
        }

        /// <summary>
        /// The coroutine that actually handles all the work and scheduling for loading a scene.
        /// </summary>
        private IEnumerator Co_LoadSceneByName(string targetScene)
        {
            //show the loading screen.
            yield return Co_FadeCanvasGroup(0, 1, blockRaycast: true);

            //TODO: Serialize current scene now.
            //yield return SaveSystem.SaveCurrentScene();

            //unload currentScene.
            if (string.IsNullOrEmpty(currentScene) is false)
            {
                SceneManager.UnloadSceneAsync(currentScene);
            }

            //load the target scene.
            AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(targetScene);
            loadingOperation.allowSceneActivation = true;
            //wait for loading to complete
            yield return loadingOperation;
            currentScene = targetScene;

            //TODO: Initialize the newly loaded scene.
            //yield return SaveSystem.InitializeCurrentScene();

            //TODO: Initialize player objects and UI now.

            //hide the loading screen.
            yield return Co_FadeCanvasGroup(1, 0, blockRaycast: false);

            //TODO: enable player control

        }

        /// <summary>
        /// Fades the alpha of the canvasGroup via a time based lerp.
        /// </summary>
        /// <param name="from">starting alpha</param>
        /// <param name="to">end alpha</param>
        /// <param name="blockRaycast">sets canvasGroup.blocksRaycasts at the end of the fade.</param>
        private IEnumerator Co_FadeCanvasGroup(float from, float to, bool blockRaycast)
        {
            for(float t = 0; t < fadeTime; t+= Time.deltaTime)
            {
                float progress = t / fadeTime;
                canvasGroup.alpha = Mathf.Lerp(from, to, progress);
                yield return null;
            }
            //finally set the to alpha, because the for loop starts before exactly 1 progress is reached.
            canvasGroup.alpha = to;
            canvasGroup.blocksRaycasts = blockRaycast;
        }

        /// <summary>
        /// Starts loading a specific scene.
        /// </summary>
        /// <param name="sceneName"></param>
        public static void LoadSceneByName(string sceneName) 
            => instance?.StartLoadingSceneByName(sceneName);
    }
}